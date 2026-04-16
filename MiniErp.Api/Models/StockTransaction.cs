using System;

namespace MiniErp.Api.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// Loại giao dịch: "IN" hoặc "OUT"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Tham chiếu (ví dụ: mã đơn hàng)
        /// </summary>
        public string Reference { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
