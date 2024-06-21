

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



public class GetReviewDto {
    public int Id { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }

}
public class ReviewService : IReviewService
{
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetReviewDto> CreateReviewAsync(ReviewDto reviewDto, string userId)
        {
            var review = new ReviewModel
            {
                UserId = userId,
                ProductId = reviewDto.ProductId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            
            return new GetReviewDto
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId,
                UserName = review.User.UserName
            };

        }


        public async Task<List<GetReviewDto>> GetReviewsForProductAsync(int productId)
        {
            var reviews = await _context.Reviews.
            Where(x => x.ProductId == productId).
            Select
            (x => new GetReviewDto
            {
                Id = x.Id,
                Comment = x.Comment,
                Rating = x.Rating,
                CreatedAt = x.CreatedAt,
                UserId = x.UserId,
                UserName = x.User.UserName
            }).
            ToListAsync();
            return reviews;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

}