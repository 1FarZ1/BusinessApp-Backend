



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;



public class AddFavoriteDto
{
    [Required]
    public int ProductId { get; set; }


}




[Authorize(policy: "User")]
[Route("api/[controller]")]
[ApiController]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoriteController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetFavorites(    )
    {
        // GetFavoriteDto[]? favorites = await _favoriteService.GetFavoritesAsync(
            // pageIndex,
            // pageSize
        // );

        return Ok("favorites");
    }

    [HttpPost("")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteDto model)
    {   

        var userId    = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var favorite = await _favoriteService.AddUserFavoriteAsync(userId,model.ProductId);
        if (favorite == false)
        {
            return BadRequest();
        }
        return Ok(favorite);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFavorite(int id)
        {

            var userId    = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _favoriteService.RemoveUserFavoriteAsync(
            userId,
            id
        );
        if (result)
        {
            return Ok(new { Message = "Favorite removed successfully!" });
        }
        return BadRequest();
    }
   
}
