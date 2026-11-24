using Findora.ViewModels.Places;

namespace Findora.Services.Interfaces
{
    public enum ReviewResult
    {
        Success,
        PlaceNotFound,
        ReviewNotFound,
        AlreadyReviewed
    }

    public interface IReviewService
    {
        Task<ReviewResult> CreateAsync(string userId, ReviewFormViewModel model);

        Task<ReviewResult> EditAsync(int reviewId, string userId, ReviewFormViewModel model);
        Task<ReviewResult> DeleteAsync(int reviewId, string userId);

    }
}