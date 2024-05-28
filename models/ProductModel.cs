
using System.ComponentModel.DataAnnotations;

public class ProductModel
{

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(1000)]
    [MinLength(3)]
    public required string Description { get; set; }


    [Required]
    [Range(0.01, 1000000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
}
