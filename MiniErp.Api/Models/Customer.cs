using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Xác nhận đây là cột tự tăng
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}