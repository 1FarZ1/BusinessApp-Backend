
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    

    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }


    public virtual ICollection<OrderItemModel> OrderItems { get; set; } // Virtual navigation property


    [Required]
    public int SubCategoryId { get; set; }


    [ForeignKey("SubCategoryId")]
    public virtual SubCategoryModel SubCategory { get; set; } // Virtual navigation property




    public virtual ICollection<UserFavoriteModel> UserFavorites { get; set; } 


    public virtual ICollection<ReviewModel> ProductReviews { get; set; }
}
