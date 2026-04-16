namespace MiniErp.Api.Dtos
{
    public class SalesOrderReadDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SalesOrderItemReadDto> Items { get; set; }
    }

    public class SalesOrderItemReadDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
