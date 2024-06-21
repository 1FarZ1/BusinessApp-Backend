

using System.ComponentModel.DataAnnotations;

public class CategoryDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; }

    public string ImageUrl { get; set; }
}

public class SubCategoryDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }
}


public interface ICategoryService
{
    Task<List<CategoryModel>> GetCategoriesAsync(
        );

    Task<List<SubCategoryModel>> GetSubCategoriesAsync(int categoryId);

    Task<List<ProductModel>> GetProductsAsync(int subCategoryId);

    Task<CategoryModel> AddCategoryAsync(CategoryDto categoryDto);

    Task<SubCategoryModel> AddSubCategoryAsync(SubCategoryDto subCategoryDto);


    Task<bool> DeleteCategoryAsync(int categoryId);

    Task<CategoryModel> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto);
    

}