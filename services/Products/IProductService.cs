using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IProductService
{
    Task<ProductModel[]> GetProductsAsync();
    Task<ProductModel?> GetProductAsync(int id);
    Task<ProductModel> AddProductAsync(ProductModel product);
    Task<ProductModel> UpdateProductAsync(int id, ProductModel product);
    Task<bool> DeleteProductAsync(int id);
}
