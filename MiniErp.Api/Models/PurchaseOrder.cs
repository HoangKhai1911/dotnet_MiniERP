namespace MiniErp.Api.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string Status { get; set; } = "Draft"; // Draft, Received
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PurchaseOrderItem> Items { get; set; }
    }
}
