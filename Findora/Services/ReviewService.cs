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


        public async Task<ReviewResult> CreateAsync(string userId, ReviewFormViewModel model)
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

        public async Task<ReviewResult> EditAsync(int reviewId, string userId, ReviewFormViewModel model)
        {
            var reviewToEdit = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Id == reviewId);

            if (reviewToEdit == null)
                return ReviewResult.ReviewNotFound;

            reviewToEdit.Rating = model.Rating;
            reviewToEdit.Title = model.Title;
            reviewToEdit.Comment = model.Comment;

            await _context.SaveChangesAsync();
            return ReviewResult.Success;

        }

        public async Task<ReviewResult> DeleteAsync(int reviewId, string userId)
        {
            var reviewToDelete = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Id == reviewId);


            if (reviewToDelete == null)
                return ReviewResult.ReviewNotFound;

            _context.Reviews.Remove(reviewToDelete);

            await _context.SaveChangesAsync();

            return ReviewResult.Success;
        }

    }
}
