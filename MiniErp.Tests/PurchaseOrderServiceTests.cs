using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Services;
using MiniErp.Api.Models;
using Xunit;

public class PurchaseOrderServiceTests
{
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new AppDbContext(options);
        db.Products.Add(new Product
        {
            Id = 1,
            SKU = "P001",
            Name = "Áo",
            Description = "Test product",
            Price = 100,
            ImageUrl = "http://example.com/product.png"
        });
        db.Warehouses.Add(new Warehouse { Id = 1, Name = "Kho chính", Location = "Hanoi" });
        db.Suppliers.Add(new Supplier { Id = 1, Name = "Nhà cung cấp A" });
        db.SaveChanges();
        return db;
    }

    [Fact]
    public async Task CreateAndReceiveOrder_UpdatesStatus()
    {
        using var db = CreateDb();
        var invSvc = new InventoryService(db);
        var svc = new PurchaseOrderService(db, invSvc);

        var dto = new PurchaseOrderCreateDto
        {
            SupplierId = 1,
            Items = new List<PurchaseOrderItemDto> { new PurchaseOrderItemDto { ProductId = 1, Quantity = 5, UnitPrice = 100 } }
        };

        var order = await svc.CreateAsync(dto);
        Assert.Equal("Draft", order.Status);

        var received = await svc.ReceiveAsync(order.Id);
        Assert.Equal("Received", received.Status);
    }
}
