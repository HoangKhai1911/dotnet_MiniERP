namespace MiniErp.Api.Dtos
{
    public class PurchaseOrderReadDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PurchaseOrderItemReadDto> Items { get; set; }
    }

    public class PurchaseOrderItemReadDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
