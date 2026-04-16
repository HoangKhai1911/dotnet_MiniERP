using Microsoft.AspNetCore.Identity;
using MiniErp.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniErp.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminEmail = "admin@mini-erp.local";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        public static void SeedData(AppDbContext db)
        {
            // 1. Categories (5 danh mục)
            if (!db.Categories.Any())
            {
                db.Categories.AddRange(
                    new Category { Name = "Điện tử & Công nghệ" },  // 1
                    new Category { Name = "Thời trang & Phụ kiện" }, // 2
                    new Category { Name = "Thực phẩm & Đồ uống" },   // 3
                    new Category { Name = "Nội thất & Nhà cửa" },    // 4
                    new Category { Name = "Sách & Văn phòng phẩm" }  // 5
                );
                db.SaveChanges(); // Lưu để lấy ID
            }

            // 2. Products (18 sản phẩm)
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    // Điện tử (1)
                    new Product { SKU = "LAP-DELL-01", Name = "Laptop Dell XPS 15", Description = "Core i7, 16GB RAM, 512GB SSD", Price = 35000000, CategoryId = 1 },
                    new Product { SKU = "LAP-MAC-01", Name = "MacBook Pro M3", Description = "Apple M3 Pro, 18GB RAM", Price = 49000000, CategoryId = 1 },
                    new Product { SKU = "PHO-IPH-15", Name = "iPhone 15 Pro Max", Description = "256GB, Titan Tự nhiên", Price = 29000000, CategoryId = 1 },
                    new Product { SKU = "PHO-SAM-24", Name = "Samsung Galaxy S24 Ultra", Description = "AI Features, 512GB", Price = 31000000, CategoryId = 1 },
                    new Product { SKU = "AUD-SON-01", Name = "Tai nghe Sony WH-1000XM5", Description = "Chống ồn chủ động", Price = 7500000, CategoryId = 1 },

                    // Thời trang (2)
                    new Product { SKU = "CLO-SHI-01", Name = "Áo thun nam Cotton basic", Description = "Màu trắng, Size L", Price = 150000, CategoryId = 2 },
                    new Product { SKU = "CLO-JEA-01", Name = "Quần Jean ống rộng nữ", Description = "Xanh nhạt, Size M", Price = 350000, CategoryId = 2 },
                    new Product { SKU = "CLO-SHO-01", Name = "Giày thể thao Sneaker", Description = "Êm ái, chạy bộ", Price = 850000, CategoryId = 2 },
                    new Product { SKU = "CLO-JAC-01", Name = "Áo khoác gió chống nước", Description = "2 lớp, màu đen", Price = 450000, CategoryId = 2 },

                    // Thực phẩm (3)
                    new Product { SKU = "FOO-COF-01", Name = "Cà phê Robusta rang mộc", Description = "Gói 500g", Price = 120000, CategoryId = 3 },
                    new Product { SKU = "FOO-TEA-01", Name = "Trà Oolong hộp thiếc", Description = "Trà thượng hạng 200g", Price = 250000, CategoryId = 3 },
                    new Product { SKU = "FOO-SNA-01", Name = "Bánh quy hạt sô cô la", Description = "Hộp 300g", Price = 85000, CategoryId = 3 },

                    // Nội thất (4)
                    new Product { SKU = "FUR-DES-01", Name = "Bàn làm việc thông minh", Description = "Gỗ sồi, nâng hạ độ cao", Price = 4500000, CategoryId = 4 },
                    new Product { SKU = "FUR-CHA-01", Name = "Ghế xoay văn phòng Ergonomic", Description = "Lưới thoáng khí, tựa đầu", Price = 2200000, CategoryId = 4 },
                    new Product { SKU = "FUR-CAB-01", Name = "Tủ quần áo 3 buồng", Description = "Gỗ MDF chống ẩm", Price = 5500000, CategoryId = 4 },

                    // Sách (5)
                    new Product { SKU = "BOO-SEL-01", Name = "Sách: Đắc Nhân Tâm", Description = "Bìa cứng, tái bản 2023", Price = 95000, CategoryId = 5 },
                    new Product { SKU = "BOO-NOV-01", Name = "Sách: Nhà Giả Kim", Description = "Tiểu thuyết bán chạy", Price = 75000, CategoryId = 5 },
                    new Product { SKU = "BOO-STA-01", Name = "Sổ tay bìa da cao cấp", Description = "Giấy vàng chống lóa", Price = 150000, CategoryId = 5 }
                );
                db.SaveChanges();
            }

            // 3. Warehouses
            if (!db.Warehouses.Any())
            {
                db.Warehouses.Add(new Warehouse { Name = "Kho Tổng Miền Nam", Location = "Cần Thơ" });
                db.Warehouses.Add(new Warehouse { Name = "Kho Trung Chuyển", Location = "Hồ Chí Minh" });
                db.SaveChanges();
            }

            // 4. Customers (10 Khách hàng)
            if (!db.Customers.Any())
            {
                db.Customers.AddRange(
                    new Customer { Name = "Nguyễn Văn An" },
                    new Customer { Name = "Trần Thị Bình" },
                    new Customer { Name = "Lê Hoàng Châu" },
                    new Customer { Name = "Phạm Duy Đạt" },
                    new Customer { Name = "Hoàng Thị Én" },
                    new Customer { Name = "Vũ Tuấn Phong" },
                    new Customer { Name = "Ngô Trí Hải" },
                    new Customer { Name = "Dương Ngọc Mai" },
                    new Customer { Name = "Lý Hải Yến" },
                    new Customer { Name = "Khách hàng Vãng lai" }
                );
                db.SaveChanges();
            }

            // 5. Suppliers
            if (!db.Suppliers.Any())
            {
                db.Suppliers.AddRange(
                    new Supplier { Name = "Nhà phân phối DigiWorld" },
                    new Supplier { Name = "Công ty May mặc Hòa Thọ" },
                    new Supplier { Name = "Xưởng Nội thất Đại Phát" }
                );
                db.SaveChanges();
            }

            // 6. Inventory (Tự động thêm tồn kho cho TẤT CẢ sản phẩm)
            if (!db.Inventories.Any())
            {
                var mainWarehouse = db.Warehouses.First();
                foreach (var product in db.Products)
                {
                    db.Inventories.Add(new Inventory
                    {
                        ProductId = product.Id,
                        WarehouseId = mainWarehouse.Id,
                        Quantity = new System.Random().Next(15, 200) // Random số lượng từ 15 đến 200 cho tự nhiên
                    });
                }
                db.SaveChanges();
            }

            // 7. Orders (Tạo sẵn 5 đơn hàng)
            if (!db.SalesOrders.Any())
            {
                var customers = db.Customers.ToList();
                var products = db.Products.ToList();
                var adminId = db.Users.First().Id;

                db.SalesOrders.AddRange(
                    new SalesOrder
                    {
                        CustomerId = customers[0].Id,
                        CreatedBy = adminId,
                        Status = "Completed",
                        Total = products[0].Price + products[4].Price, // Mua Laptop + Tai nghe
                        Items = new List<SalesOrderItem>
                        {
                            new SalesOrderItem { ProductId = products[0].Id, Quantity = 1, UnitPrice = products[0].Price },
                            new SalesOrderItem { ProductId = products[4].Id, Quantity = 1, UnitPrice = products[4].Price }
                        }
                    },
                    new SalesOrder
                    {
                        CustomerId = customers[1].Id,
                        CreatedBy = adminId,
                        Status = "Pending",
                        Total = products[5].Price * 3, // Mua 3 Áo thun
                        Items = new List<SalesOrderItem>
                        {
                            new SalesOrderItem { ProductId = products[5].Id, Quantity = 3, UnitPrice = products[5].Price }
                        }
                    },
                    new SalesOrder
                    {
                        CustomerId = customers[2].Id,
                        CreatedBy = adminId,
                        Status = "Shipped",
                        Total = products[12].Price + (products[13].Price * 2), // Mua 1 Bàn + 2 Ghế
                        Items = new List<SalesOrderItem>
                        {
                            new SalesOrderItem { ProductId = products[12].Id, Quantity = 1, UnitPrice = products[12].Price },
                            new SalesOrderItem { ProductId = products[13].Id, Quantity = 2, UnitPrice = products[13].Price }
                        }
                    },
                    new SalesOrder
                    {
                        CustomerId = customers[3].Id,
                        CreatedBy = adminId,
                        Status = "Completed",
                        Total = products[2].Price, // Mua iPhone
                        Items = new List<SalesOrderItem>
                        {
                            new SalesOrderItem { ProductId = products[2].Id, Quantity = 1, UnitPrice = products[2].Price }
                        }
                    },
                    new SalesOrder
                    {
                        CustomerId = customers[8].Id,
                        CreatedBy = adminId,
                        Status = "Pending",
                        Total = products[15].Price + products[16].Price, // Mua 2 cuốn sách
                        Items = new List<SalesOrderItem>
                        {
                            new SalesOrderItem { ProductId = products[15].Id, Quantity = 1, UnitPrice = products[15].Price },
                            new SalesOrderItem { ProductId = products[16].Id, Quantity = 1, UnitPrice = products[16].Price }
                        }
                    }
                );
                db.SaveChanges();
            }
        }
    }
}