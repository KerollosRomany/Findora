using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Places;
using Microsoft.EntityFrameworkCore;

namespace Findora.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
        public ReviewService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ReviewResult> CreateAsync(string userId, CreateReviewViewModel model)
        {

            var placeExists = await _context.Places.AnyAsync(p => p.Id == model.PlaceId);
            if (!placeExists) return ReviewResult.PlaceNotFound;

            var alreadyReviewed = await _context.Reviews
              .AnyAsync(r => r.UserId == userId && r.PlaceId == model.PlaceId);
            if (alreadyReviewed) return ReviewResult.AlreadyReviewed;

            var reviewToAdd = new Review
            {
                PlaceId = model.PlaceId,
                UserId = userId,
                Rating = model.Rating,
                Title = model.Title,
                Comment = model.Comment,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Reviews.Add(reviewToAdd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return ReviewResult.AlreadyReviewed;
            }
            return ReviewResult.Success;
        }
    }
}
