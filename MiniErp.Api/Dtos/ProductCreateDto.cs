using System.ComponentModel.DataAnnotations;

namespace MiniErp.Api.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string SKU { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public string ImageUrl { get; set; }
    }
}
