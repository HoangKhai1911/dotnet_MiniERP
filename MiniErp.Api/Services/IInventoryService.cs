using System.Collections.Generic;
using System.Threading.Tasks;
using MiniErp.Api.Models;

namespace MiniErp.Api.Services
{
    public interface IInventoryService
    {
        // 2 hàm mới thêm vào để lấy dữ liệu hiển thị (Đã sửa đổi kiểu trả về thành IEnumerable)
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId);

        // Hàm cũ của bạn giữ nguyên để điều chỉnh kho
        Task<Inventory> AdjustStockAsync(int productId, int warehouseId, int quantity, string type, string reference);
    }
}