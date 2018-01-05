using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Models;
using Store.Web.Models.FeedbackViewModels;
using Store.Services.Interfaces;
using Store.Services.Implementations;
using AutoMapper;
using System.Threading.Tasks;
using System;
using Store.Services.Models.FeedbackViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Store.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IFeedbackService feedbackService;

        public FeedbackController(UserManager<User> userManager,
            IFeedbackService feedbackService)
        {
            this.userManager = userManager;
            this.feedbackService = feedbackService;
        }

        [HttpGet]
        public IActionResult Create(string receiverId, int productId)
        {
            var loggedUserId = this.userManager.GetUserId(User);

            var model = new CreateFeedbackViewModel
            {
                SenderId = loggedUserId,
                ReceiverId = receiverId,
                ProductId = productId
            };

            return View(nameof(Create), model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFeedbackViewModel model)
        {
            var feedback = Mapper.Map<Feedback>(model);

            var creationMessage = await this.feedbackService.CreateAsync(feedback);
            if (creationMessage != null)
            {
                TempData[WebConstants.WarningMessageKey] = creationMessage;
                return View(model);
            }

            TempData[WebConstants.SuccessMessageKey] = "Feedback was posted Successfully";
            return RedirectToAction(nameof(Details), new { feedback.ProductId, feedback.SenderId });
        }

        public async Task<IActionResult> Details(int productId, string senderId)
        {
            var feedback = await this.feedbackService.GetFeedbackAsync<DetailsFeedbackViewModel>(productId, senderId);

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int productId, string senderId)
        {
            var feedback = await this.feedbackService.GetFeedbackAsync<DetailsFeedbackViewModel>(productId, senderId);

            if (feedback == null)
            {
                return NotFound();
            }

            if (feedback.SenderId != senderId)
            {
                TempData[WebConstants.WarningMessageKey] = "You cannot edit someone else's feedback";
                return RedirectToAction("FeedbacksList");
            }

            return View(feedback);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DetailsFeedbackViewModel model)
        {
            var loggedUserId = this.userManager.GetUserId(User);

            if (model.SenderId != loggedUserId)
            {
                TempData[WebConstants.WarningMessageKey] = "You cannot delete someone else's feedback";
                return RedirectToAction("FeedbacksList");
            }

            var editedFeedback = await this.feedbackService.EditFeedback(model, loggedUserId);
            var mapped = Mapper.Map<Feedback>(editedFeedback);

            return RedirectToAction("Details", new { model.ProductId, model.SenderId });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int productId, string senderId)
        {
            var feedbackToDelete = await this.feedbackService.GetFeedbackAsync(productId, senderId);
            var loggedUserId = this.userManager.GetUserId(User);

            if (feedbackToDelete == null)
            {
                TempData[WebConstants.WarningMessageKey] = "There is no such feedback to be deleted";
                return RedirectToAction("FeedbacksList");
            }

            if (feedbackToDelete.SenderId != loggedUserId)
            {
                TempData[WebConstants.WarningMessageKey] = "You cannot delete someone else's feedback";
                return RedirectToAction("FeedbacksList");
            }

            this.feedbackService.Delete(feedbackToDelete);

            TempData[WebConstants.SuccessMessageKey] = "Feedback was successfully deleted";
            return RedirectToAction("FeedbacksList");
        }

        public IActionResult FeedbacksList(int productId)
        {
            if (productId == 0)
            {
                var loggedUserId = this.userManager.GetUserId(User);
                var userFeedbacks = this.feedbackService.GetUserFeedbacks(loggedUserId);

                return View(userFeedbacks);
            }

            var productFeedbacks = this.feedbackService.GetProductFeedbacks(productId);

            return View(productFeedbacks);
        }
    }
}