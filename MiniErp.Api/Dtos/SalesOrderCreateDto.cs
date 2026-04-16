using System.ComponentModel.DataAnnotations;

namespace MiniErp.Api.Dtos
{
    public class SalesOrderCreateDto
    {
        public int? CustomerId { get; set; }

        [Required]
        public List<SalesOrderItemDto> Items { get; set; }
    }

    public class SalesOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
