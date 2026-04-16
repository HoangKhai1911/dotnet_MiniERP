using MiniErp.Api.Dtos;

namespace MiniErp.Api.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderReadDto> CreateAsync(PurchaseOrderCreateDto dto);
        Task<PurchaseOrderReadDto> ReceiveAsync(int orderId);
        Task<PurchaseOrderReadDto> GetByIdAsync(int id);
    }
}
