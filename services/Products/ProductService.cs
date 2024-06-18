using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ProductService  : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductModel[]> GetProductsAsync()
    {
        return await _context.Products.ToArrayAsync();
    }

    public async Task<ProductModel?> GetProductAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
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
}
