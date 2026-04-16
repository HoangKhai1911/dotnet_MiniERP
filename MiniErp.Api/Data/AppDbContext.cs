using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Models;

namespace MiniErp.Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Products & Categories
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Warehouses & Inventory
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        // Orders
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        // Business partners
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Cấu hình độ chính xác cho các trường tiền tệ (Decimal)
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<SalesOrderItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<SalesOrder>()
                .Property(s => s.Total)
                .HasPrecision(18, 2);

            // 2. NGĂN CHẶN XÓA DÂY CHUYỀN (CASCADE DELETE) CHO CÁC DỮ LIỆU QUAN TRỌNG

            // Không cho phép xóa Product nếu nó đã nằm trong Đơn hàng hoặc Tồn kho
            builder.Entity<SalesOrderItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseOrderItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StockTransaction>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không cho phép xóa Customer/Supplier nếu họ đã có Đơn hàng
            builder.Entity<SalesOrder>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseOrder>()
                .HasOne<Supplier>()
                .WithMany()
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}