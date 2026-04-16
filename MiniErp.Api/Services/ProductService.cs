using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Models;

namespace MiniErp.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
        {
            var exists = await _db.Products.AnyAsync(p => p.SKU == dto.SKU);
            if (exists) throw new InvalidOperationException("SKU already exists");

            var product = new Product
            {
                SKU = dto.SKU,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return new ProductReadDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                ImageUrl = product.ImageUrl
            };
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            var query = _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = await query.ToListAsync();

            return list.Select(p => new ProductReadDto
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl
            });
        }

        public async Task<ProductReadDto> GetByIdAsync(int id)
        {
            var p = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return null;

            return new ProductReadDto
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImageUrl
            };
        }
    }
}
