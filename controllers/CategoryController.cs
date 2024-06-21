using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;





[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCategories(
    )
    {
        List<CategoryModel> categories = await _categoryService.GetCategoriesAsync();
        if (categories == null)
        {
            return NotFound();
        }
        return Ok(categories);

    }

    [HttpGet("sub/{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        List<SubCategoryModel> categories = await _categoryService.GetSubCategoriesAsync(id);
        if (categories == null)
        {
            return NotFound();
        }

        return Ok(categories);
    }

    [Authorize(policy: "Admin")]
    [HttpPost("")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto model)
    {
        var category = await _categoryService.AddCategoryAsync(model);
        if (category == null)
        {
            return BadRequest();
        }
        return Ok(category);
    }

    [Authorize(policy: "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto model)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, model);
        if (category == null)
        {
            return BadRequest();
        }
        return Ok(category);
    }

    [Authorize(policy: "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (!result)
        {
            return BadRequest();
        }
        return Ok();

    }

   [Authorize(policy: "Admin")]
    [HttpPost("sub")]
    public async Task<IActionResult> AddSubCategory([FromBody] SubCategoryDto model)
    {
        var category = await _categoryService.AddSubCategoryAsync(model);
        if (category == null)
        {
            return BadRequest();
        }
        return Ok(category);
    }

    [HttpGet("sub/products/{subId}")]
    public async Task<IActionResult> GetProducts(int subId)
    {
        var products = await _categoryService.GetProductsAsync(subId);
        if (products == null)
        {
            return NotFound();
        }
        return Ok(products);
    }
   
}
