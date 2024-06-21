

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public List<SubCategoryModel> SubCategories { get; set; }
}

public class SubCategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; }
    public List<ProductModel> Products { get; set; }

    
}