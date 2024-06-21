

public interface    IReviewService
{
    Task<GetReviewDto> CreateReviewAsync(ReviewDto reviewDto, string userId);
    // Task<ReviewDto> UpdateReviewAsync(ReviewDto reviewDto);
    Task<bool> DeleteReviewAsync(int reviewId);
    Task<List<GetReviewDto>> GetReviewsForProductAsync(
        int productId);
}