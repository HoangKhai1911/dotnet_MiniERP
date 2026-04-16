using MiniErp.Api.Dtos;

namespace MiniErp.Api.Services
{
    public interface IProductService
    {
        Task<ProductReadDto> CreateAsync(ProductCreateDto dto);
        Task<IEnumerable<ProductReadDto>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<ProductReadDto> GetByIdAsync(int id);
    }
}
