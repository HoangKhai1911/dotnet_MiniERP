using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Services;
using Xunit;

public class ProductServiceTests
{
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateProduct_Success()
    {
        using var db = CreateDb();
        var svc = new ProductService(db);

        var dto = new ProductCreateDto
        {
            SKU = "T001",
            Name = "Test",
            Description = "Test product",
            Price = 100,
            ImageUrl = "http://example.com/test.png"
        };
        var result = await svc.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("T001", result.SKU);
    }

    [Fact]
    public async Task CreateProduct_DuplicateSKU_Throws()
    {
        using var db = CreateDb();
        var svc = new ProductService(db);

        var dto = new ProductCreateDto
        {
            SKU = "T002",
            Name = "A",
            Description = "Duplicate product",
            Price = 10,
            ImageUrl = "http://example.com/duplicate.png"
        };
        await svc.CreateAsync(dto);

        await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateAsync(dto));
    }
}
