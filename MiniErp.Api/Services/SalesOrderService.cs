using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Dtos;
using MiniErp.Api.Models;

namespace MiniErp.Api.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _db;
        private readonly IInventoryService _inventory;
        public SalesOrderService(AppDbContext db, IInventoryService inventory)
        {
            _db = db;
            _inventory = inventory;
        }

        public async Task<SalesOrderReadDto> CreateAsync(SalesOrderCreateDto dto)
        {
            // Kiểm tra khách hàng tồn tại
            var customer = await _db.Customers.FindAsync(dto.CustomerId);
            if (customer == null) throw new InvalidOperationException("Khách hàng không tồn tại");

            var order = new SalesOrder
            {
                CustomerId = dto.CustomerId.Value,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                Items = new List<SalesOrderItem>()
            };

            decimal total = 0;
            foreach (var item in dto.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                order.Items.Add(new SalesOrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice > 0 ? item.UnitPrice : product.Price
                });
                total += item.Quantity * (item.UnitPrice > 0 ? item.UnitPrice : product.Price);
            }
            order.Total = total;

            _db.SalesOrders.Add(order);
            await _db.SaveChangesAsync();

            return await GetByIdAsync(order.Id);
        }
        public async Task<SalesOrderReadDto> ShipAsync(int orderId)
        {
            var order = await _db.SalesOrders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) throw new InvalidOperationException("Order not found");
            if (order.Status != "Pending") throw new InvalidOperationException("Order already processed");

            foreach (var item in order.Items)
            {
                await _inventory.AdjustStockAsync(item.ProductId, 1, item.Quantity, "OUT", $"SalesOrder:{order.Id}");
            }

            order.Status = "Shipped";
            await _db.SaveChangesAsync();

            return await GetByIdAsync(order.Id);
        }

        public async Task<SalesOrderReadDto> GetByIdAsync(int id)
        {
            var order = await _db.SalesOrders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new SalesOrderReadDto
            {
                Id = order.Id,
                Status = order.Status,
                Total = order.Total,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new SalesOrderItemReadDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }
        public async Task<IEnumerable<SalesOrderReadDto>> GetAllAsync()
        {
            // 1. Chỉ lấy những trường chắc chắn có
            var orders = await _db.SalesOrders
                .Include(o => o.Items)
                .ToListAsync();

            return orders.Select(o => new SalesOrderReadDto
            {
                Id = o.Id,
                Status = o.Status,
                Total = o.Total,
                // Tạm thời comment dòng CreatedAt nếu bạn không chắc chắn tên cột trong DB
                // CreatedAt = o.CreatedAt, 
                Items = new List<SalesOrderItemReadDto>()
            });
        }
    }
}
