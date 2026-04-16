using Microsoft.AspNetCore.Authorization; // THÊM DÒNG NÀY
using Microsoft.AspNetCore.Mvc;
using MiniErp.Api.Services;
using System.Threading.Tasks;

namespace MiniErp.Api.Controllers
{
    [Authorize] // THÊM DÒNG NÀY: Bắt buộc phải có Token JWT mới được gọi API
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return Ok(inventory);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryByProductId(int productId)
        {
            var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);
            if (inventory == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin tồn kho cho sản phẩm này." });
            }
            return Ok(inventory);
        }
    }
}