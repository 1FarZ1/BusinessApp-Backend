using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController( IProductService productService)
    { 
        _productService = productService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {
        ProductModel[]? products = await _productService.GetProductsAsync(
            pageIndex,
            pageSize
        );
        if (products == null)
        {
            return NotFound();
        }
        return Ok(products);

    }

    //search
    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string query)
    {
        ProductModel[]? products = await _productService.SearchProductsAsync(query);
        if (products == null)
        {
            return NotFound();
        }
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        ProductModel? product = await _productService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    


    [Authorize(Roles = "Admin")]
    [HttpPost("")]   
    public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
    {
        try
        {
            ProductModel? newProduct = await _productService.AddProductAsync(product);
            return Ok(value: newProduct);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(policy: "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto product)
    {
        try
        {
            ProductModel? updatedProduct = await _productService.UpdateProductAsync(id, product);
            return Ok(value: updatedProduct);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }   

    [Authorize (policy: "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            bool result = await _productService.DeleteProductAsync(id);
            return Ok(value: result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
