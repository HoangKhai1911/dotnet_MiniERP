using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Services;
using MiniErp.Api.Models;
using Xunit;

public class SalesOrderServiceTests
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
        db.SaveChanges();
        return db;
    }

    [Fact]
    public async Task CreateAndShipOrder_UpdatesStatus()
    {
        using var db = CreateDb();
        var invSvc = new InventoryService(db);
        var svc = new SalesOrderService(db, invSvc);

        var dto = new SalesOrderCreateDto
        {
            Items = new List<SalesOrderItemDto> { new SalesOrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 } }
        };

        var order = await svc.CreateAsync(dto);
        Assert.Equal("Pending", order.Status);

        var shipped = await svc.ShipAsync(order.Id);
        Assert.Equal("Shipped", shipped.Status);
    }
}
