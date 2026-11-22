using Findora.ViewModels.Places;

namespace Findora.Services.Interfaces
{
    public enum ReviewResult
    {
        Success,
        PlaceNotFound,
        AlreadyReviewed
    };

    public interface IReviewService
    {
        Task<ReviewResult> CreateAsync(string userId, CreateReviewViewModel model);
    }
}