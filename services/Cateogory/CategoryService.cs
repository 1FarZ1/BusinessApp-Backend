





using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryModel> AddCategoryAsync(CategoryDto categoryDto)
    {
        var category = new CategoryModel
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            ImageUrl = categoryDto.ImageUrl
        };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<CategoryModel> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null) return null;

        return  category;


    }

    public async Task<List<CategoryModel>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return categories;
    }

    public async Task<CategoryModel> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.ImageUrl = categoryDto.ImageUrl;
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null) return false;
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async  Task<List<SubCategoryModel>> GetSubCategoriesAsync(int categoryId)
    {
         var subCategories =await  _context.SubCategories.Where(x => x.CategoryId == categoryId).ToListAsync();
        return subCategories;
    }

    public async Task<List<ProductModel>> GetProductsAsync(int subCategoryId)
    {
        var products = await _context.Products.Where(x => x.SubCategoryId == subCategoryId).ToListAsync();
        return products;
    }

    public async Task<SubCategoryModel>  AddSubCategoryAsync(SubCategoryDto subCategoryDto)
    {
        var subCategory = new SubCategoryModel
        {
            Name = subCategoryDto.Name,
            Description = subCategoryDto.Description,
            ImageUrl = subCategoryDto.ImageUrl,
            CategoryId = subCategoryDto.CategoryId
        };
        await _context.SubCategories.AddAsync(subCategory);
        await _context.SaveChangesAsync();
        return subCategory;
    }
}