using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Places;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Findora.Controllers
{
    [Authorize(Roles = AppRoles.Student)]
    public class ReviewsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReviewService _reviewService;

        public ReviewsController(UserManager<ApplicationUser> userManager, IReviewService reviewService) 
        {
            _userManager = userManager;
            _reviewService = reviewService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewFormViewModel reviewViewModelFromRequest)
        {
            if(!ModelState.IsValid) 
            {
                TempData["Error"] = "Please fix the errors and try again.";
                return RedirectToAction("Details", "Places", new {id = reviewViewModelFromRequest.PlaceId, fragment = "reviews" });
            }

            var userId = _userManager.GetUserId(User);
            var result = await _reviewService.CreateAsync(userId, reviewViewModelFromRequest);
            switch (result)
            {
                case ReviewResult.Success:
                    TempData["Success"] = "Review Created Successfully.";
                    break;
                case ReviewResult.AlreadyReviewed:
                    TempData["Error"] = "You already made a review before.";
                    break;
                case ReviewResult.PlaceNotFound:
                    TempData["Error"] = "Place not Exists.";
                    break;
            }
            return RedirectToAction("Details", "Places", new { id = reviewViewModelFromRequest.PlaceId }, "reviews");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReviewFormViewModel reviewViewModelFromRequest, int reviewId)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors and try again.";
                return RedirectToAction(
                    "Details",
                    "Places",
                    new { id = reviewViewModelFromRequest.PlaceId, fragment = "reviews" });
            }
            var userId = _userManager.GetUserId(User);
            var result = await _reviewService.EditAsync(reviewId, userId, reviewViewModelFromRequest);
            switch (result)
            {
                case ReviewResult.Success:
                    TempData["Success"] = "Review Updated Successfully.";
                    break;
                case ReviewResult.ReviewNotFound:
                    TempData["Error"] = "Review not found!";
                    break;

            }
            return RedirectToAction("Details", "Places", new { id = reviewViewModelFromRequest.PlaceId }, "reviews");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int reviewId, int placeId)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _reviewService.DeleteAsync(reviewId, userId);
            switch (result)
            {
                case ReviewResult.Success:
                    TempData["Success"] = "Review Deleted Successfully.";
                    break;
                case ReviewResult.ReviewNotFound:
                    TempData["Error"] = "Review not found!";
                    break;

            }
            return RedirectToAction("Details", "Places", new { id = placeId, fragment = "reviews" });
        }
    }
}
