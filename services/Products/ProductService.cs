using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}


public class ProductService  : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

 public async Task<ProductViewModel[]> GetProductsAsync(int pageIndex, int pageSize)
{
    var products = await _context.Products
        .OrderBy(p => p.Name)
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        })
        .ToArrayAsync();
    return products;
}

    public async Task<ProductViewModel?> GetProductAsync(int id)
    {
        return await _context.Products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        })
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductModel> AddProductAsync( ProductDto productDto)
    {
        var product = new ProductModel
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<ProductModel> UpdateProductAsync(int id, ProductDto product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existingProduct == null)
        {
            throw new Exception("Product not found");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existingProduct == null)
        {
            throw new Exception("Product not found");
        }

        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ProductModel[]> SearchProductsAsync(string query)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
            .ToArrayAsync();
    }
}
