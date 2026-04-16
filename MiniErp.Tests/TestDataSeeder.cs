using System;
using System.Linq;
using MiniErp.Api.Data;
using MiniErp.Api.Models;

namespace MiniErp.Tests
{
    public static class TestDataSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // Category
            var category = new Category { Id = 1, Name = "Default Category" };
            db.Categories.Add(category);

            // Customer
            var customer = new Customer { Id = 1, Name = "Default Customer" };
            db.Customers.Add(customer);

            // Supplier
            var supplier = new Supplier { Id = 1, Name = "Default Supplier" };
            db.Suppliers.Add(supplier);

            // Warehouse
            var warehouse = new Warehouse { Id = 1, Name = "Main Warehouse", Location = "Can Tho" };
            db.Warehouses.Add(warehouse);

            // Products
            db.Products.AddRange(Enumerable.Range(1, 5).Select(i => new Product
            {
                Id = i,
                SKU = $"SKU{i}",
                Name = $"Product {i}",
                Description = $"Description {i}",
                ImageUrl = $"http://example.com/img{i}.png",
                CategoryId = category.Id
            }));

            db.SaveChanges();

            // Inventory
            db.Inventories.AddRange(db.Products.Select(p => new Inventory
            {
                ProductId = p.Id,
                WarehouseId = warehouse.Id,
                Quantity = 100
            }));

            db.SaveChanges();

            // Sales Order
            var salesOrder = new SalesOrder
            {
                Id = 1,
                CustomerId = customer.Id,
                Status = "Pending",
                Items = db.Products.Take(2).Select(p => new SalesOrderItem
                {
                    ProductId = p.Id,
                    Quantity = 2
                }).ToList()
            };
            db.SalesOrders.Add(salesOrder);

            // Purchase Order
            var purchaseOrder = new PurchaseOrder
            {
                Id = 1,
                SupplierId = supplier.Id,
                Status = "Created",
                Items = db.Products.Take(2).Select(p => new PurchaseOrderItem
                {
                    ProductId = p.Id,
                    Quantity = 5
                }).ToList()
            };
            db.PurchaseOrders.Add(purchaseOrder);

            db.SaveChanges();
        }
    }
}
