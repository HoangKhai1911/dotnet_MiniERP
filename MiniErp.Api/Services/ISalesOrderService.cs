using System.Collections.Generic;
using System.Threading.Tasks;
using MiniErp.Api.Dtos;

namespace MiniErp.Api.Services
{
    public interface ISalesOrderService
    {
        // Thêm dòng này để khai báo hàm lấy tất cả đơn hàng
        Task<IEnumerable<SalesOrderReadDto>> GetAllAsync();

        Task<SalesOrderReadDto> CreateAsync(SalesOrderCreateDto dto);
        Task<SalesOrderReadDto> ShipAsync(int orderId);
        Task<SalesOrderReadDto> GetByIdAsync(int id);
    }
}