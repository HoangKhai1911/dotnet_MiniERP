using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniErp.Api.Data;
using MiniErp.Api.Models;

namespace MiniErp.Api.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _db;

        public InventoryService(AppDbContext db)
        {
            _db = db;
        }

        // --- 1. HÀM MỚI: Lấy toàn bộ tồn kho ---
        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _db.Inventories
                .Include(i => i.Product)   // Join để lấy Tên sản phẩm
                .Include(i => i.Warehouse) // Join để lấy Tên kho
                .ToListAsync();
        }

        // --- 2. HÀM MỚI: Lấy tồn kho của 1 sản phẩm cụ thể ---
        public async Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId)
        {
            return await _db.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        // --- 3. HÀM CŨ CỦA BẠN: Điều chỉnh số lượng kho ---
        public async Task<Inventory> AdjustStockAsync(int productId, int warehouseId, int quantity, string type, string reference)
        {
            var inventory = await _db.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    ProductId = productId,
                    WarehouseId = warehouseId,
                    Quantity = 0
                };
                _db.Inventories.Add(inventory);
            }

            if (type == "IN")
                inventory.Quantity += quantity;
            else if (type == "OUT")
                inventory.Quantity -= quantity;

            _db.StockTransactions.Add(new StockTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                Type = type,
                Reference = reference,
                CreatedAt = DateTime.UtcNow // Đã thêm using System; ở trên cùng để không bị lỗi
            });

            await _db.SaveChangesAsync();
            return inventory;
        }
    }
}