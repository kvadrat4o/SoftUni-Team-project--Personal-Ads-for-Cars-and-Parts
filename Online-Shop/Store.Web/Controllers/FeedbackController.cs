using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Models;
using Store.Web.Models.FeedbackViewModels;
using Store.Services.Interfaces;
using Store.Services.Implementations;
using AutoMapper;
using System.Threading.Tasks;

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

            return View(feedback);
        }
    }
}