using System.ComponentModel.DataAnnotations;

namespace MiniErp.Api.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
