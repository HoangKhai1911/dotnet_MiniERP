using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Services;
using MiniErp.Api.Models;
using Xunit;

public class InventoryServiceTests
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
    public async Task AdjustStock_IN_IncreasesQuantity()
    {
        using var db = CreateDb();
        var svc = new InventoryService(db);

        var result = await svc.AdjustStockAsync(1, 1, 5, "IN", "Test");
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public async Task AdjustStock_OUT_DecreasesQuantity()
    {
        using var db = CreateDb();
        var svc = new InventoryService(db);

        await svc.AdjustStockAsync(1, 1, 10, "IN", "Init");
        var result = await svc.AdjustStockAsync(1, 1, 3, "OUT", "Ship");
        Assert.Equal(7, result.Quantity);
    }
}
