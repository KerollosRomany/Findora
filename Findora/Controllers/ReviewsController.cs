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
        public async Task<IActionResult> Create(CreateReviewViewModel reviewViewModelFromRequest)
        {
            if(!ModelState.IsValid) 
            {
                TempData["Error"] = "Please fix the errors and try again.";
                return RedirectToAction("Details", "Places", new {id = reviewViewModelFromRequest.PlaceId}, "reviews");
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
    }
}
