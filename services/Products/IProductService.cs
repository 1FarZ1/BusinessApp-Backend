using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IProductService
{
    Task<ProductViewModel[]> GetProductsAsync(int pageIndex, int pageSize);
    Task<ProductViewModel?> GetProductAsync(int id);
    Task<ProductModel> AddProductAsync(ProductDto product);
    Task<ProductModel> UpdateProductAsync(int id, ProductDto product);
    Task<bool> DeleteProductAsync(int id);

    Task<ProductModel[]> SearchProductsAsync(string query);
}
