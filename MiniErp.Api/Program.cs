using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // <-- 1. ĐÃ THÊM THƯ VIỆN NÀY CHO SWAGGER
using Serilog;
using System.Data;
using System.Text;
using MiniErp.Api.Data;
using MiniErp.Api.Models;
using MiniErp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Logging (Serilog)
builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
      .ReadFrom.Configuration(ctx.Configuration));

var configuration = builder.Configuration;

// 2. Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// 3. Identity System
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 4. JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "ReplaceWithAStrongSecretKey12345");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 5. CORS Configuration (Cho phép React Frontend gọi API) - ĐÃ DỌN DẸP CHỈ GIỮ 1 CÁI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // Port của Vite/React
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// 6. Dependency Injection (Services)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Giúp tránh lỗi vòng lặp khi quan hệ DB phức tạp
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// 7. CẤU HÌNH SWAGGER (ĐÃ THÊM NÚT AUTHORIZE)
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header. \r\n\r\n
                      Nhập 'Bearer' [khoảng trắng] và dán Token của bạn vào.\r\n\r\n
                      Ví dụ: 'Bearer eyJhbGciOiJIUzI1NiIs...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 8. Database Seeding & Initialization Logic
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Helper: Kiểm tra bảng tồn tại
    bool TableExists(AppDbContext context, string tableName)
    {
        var connection = context.Database.GetDbConnection();
        try
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@tableName";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        catch { return false; }
        finally { if (connection.State == ConnectionState.Open) connection.Close(); }
    }

    // Helper: Seed dữ liệu mẫu
    void SeedDatabase(AppDbContext context)
    {
        if (!context.Roles.Any(r => r.Name == "Admin"))
        {
            context.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
        }
        if (!context.Roles.Any(r => r.Name == "User"))
        {
            context.Roles.Add(new IdentityRole { Name = "User", NormalizedName = "USER" });
        }
        context.SaveChanges();

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(new Category { Name = "Electronics" }, new Category { Name = "Clothing" });
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.Add(new Product { SKU = "SKU001", Name = "Laptop", Price = 1200, CategoryId = 1 });
            context.SaveChanges();
        }

        if (!context.Warehouses.Any())
        {
            context.Warehouses.Add(new Warehouse { Name = "Main Warehouse", Location = "Can Tho" });
            context.SaveChanges();
        }
    }

    try
    {
        Log.Information("Đang khởi tạo Database...");
        db.Database.Migrate();

        if (TableExists(db, "AspNetRoles"))
        {
            // Seed Admin User (Chỉ gọi 1 lần duy nhất ở đây)
            await DbInitializer.SeedAdminAsync(userManager, roleManager);
            SeedDatabase(db);
            Log.Information("Khởi tạo Database thành công.");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi khởi tạo Database. Đang thử xóa và tạo lại...");
        if (app.Environment.IsDevelopment())
        {
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            await DbInitializer.SeedAdminAsync(userManager, roleManager);
            SeedDatabase(db);
        }
    }
}

// 9. Middleware Pipeline (Thứ tự rất quan trọng)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

// UseCors phải nằm TRƯỚC Authentication
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();