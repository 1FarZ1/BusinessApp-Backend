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
    public async Task<IActionResult> GetProducts()
    {
        ProductModel[]? products = await _productService.GetProductsAsync();
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


    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductModel product)
    {
        ProductModel? newProduct = await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
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
