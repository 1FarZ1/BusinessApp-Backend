



using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ReviewDto
{
    public int ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IAuthService _authService;

    public ReviewController(IReviewService reviewService, IAuthService authService)
    {
        _reviewService = reviewService;
        _authService = authService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var review = await _reviewService.CreateReviewAsync(reviewDto, userId);
        if (review == null)
        {
            return BadRequest();
        }

        return Ok(review);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetReviews(int productId)
    {
        var reviews = await _reviewService.GetReviewsForProductAsync(productId);
        if (reviews == null)
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpDelete("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var userId =  User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        

        var result = await _reviewService.DeleteReviewAsync(reviewId);
        if(!result)
        {
            return BadRequest();
        }

        return Ok("Review deleted successfully!");
    }
}