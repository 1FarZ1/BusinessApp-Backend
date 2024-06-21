

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }



    public virtual ICollection<SubCategoryModel> SubCategories { get; set; }
    
    }

public class SubCategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }


    [Required]
    public int CategoryId { get; set; }


    [ForeignKey("CategoryId")]
    public virtual CategoryModel Category { get; set; }

    public virtual ICollection<ProductModel> Products { get; set; }


}