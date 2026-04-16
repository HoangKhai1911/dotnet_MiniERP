using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniErp.Api.Dtos;
using MiniErp.Api.Services;
using System;
using System.Threading.Tasks;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _svc;
        public SalesOrdersController(ISalesOrderService svc) { _svc = svc; }

        // --- HÀM VỪA ĐƯỢC THÊM VÀO ĐỂ LẤY DANH SÁCH ---
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Giả sử ISalesOrderService của bạn có hàm GetAllAsync()
            var orders = await _svc.GetAllAsync();
            return Ok(orders);
        }
        // ----------------------------------------------

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] SalesOrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPost("{id:int}/ship")]
        [Authorize]
        public async Task<IActionResult> Ship(int id)
        {
            try
            {
                var order = await _svc.ShipAsync(id);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _svc.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}