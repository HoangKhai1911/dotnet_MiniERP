using System.ComponentModel.DataAnnotations;

namespace MiniErp.Api.Dtos
{
    public class PurchaseOrderCreateDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public List<PurchaseOrderItemDto> Items { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
