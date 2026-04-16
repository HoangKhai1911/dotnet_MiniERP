using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniErp.Api.Dtos;
using MiniErp.Api.Services;

namespace MiniErp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _svc;
        public PurchaseOrdersController(IPurchaseOrderService svc) { _svc = svc; }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPost("{id:int}/receive")]
        [Authorize]
        public async Task<IActionResult> Receive(int id)
        {
            try
            {
                var order = await _svc.ReceiveAsync(id);
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
