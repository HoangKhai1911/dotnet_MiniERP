using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Models;

namespace MiniErp.Api.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly AppDbContext _db;
        private readonly IInventoryService _inventory;
        public PurchaseOrderService(AppDbContext db, IInventoryService inventory)
        {
            _db = db;
            _inventory = inventory;
        }

        public async Task<PurchaseOrderReadDto> CreateAsync(PurchaseOrderCreateDto dto)
        {
            var order = new PurchaseOrder
            {
                SupplierId = dto.SupplierId,
                Status = "Draft",
                Items = new List<PurchaseOrderItem>()
            };

            foreach (var item in dto.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) throw new InvalidOperationException("Product not found");

                order.Items.Add(new PurchaseOrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _db.PurchaseOrders.Add(order);
            await _db.SaveChangesAsync();

            return await GetByIdAsync(order.Id);
        }

        public async Task<PurchaseOrderReadDto> ReceiveAsync(int orderId)
        {
            var order = await _db.PurchaseOrders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) throw new InvalidOperationException("Order not found");
            if (order.Status != "Draft") throw new InvalidOperationException("Order already received");

            foreach (var item in order.Items)
            {
                await _inventory.AdjustStockAsync(item.ProductId, 1, item.Quantity, "IN", $"PurchaseOrder:{order.Id}");
            }

            order.Status = "Received";
            await _db.SaveChangesAsync();

            return await GetByIdAsync(order.Id);
        }

        public async Task<PurchaseOrderReadDto> GetByIdAsync(int id)
        {
            var order = await _db.PurchaseOrders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new PurchaseOrderReadDto
            {
                Id = order.Id,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new PurchaseOrderItemReadDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }
    }
}
