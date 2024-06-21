using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

public class ProductDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(3)]
        public required string Name { get; set; } 


    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }


    [Required]
    [Range(0.01, 1000000)]

    public decimal Price { get; set; } 

    public string? ImageUrl { get; set; }

    [Required]
    public int SubCategoryId { get; set; }

}