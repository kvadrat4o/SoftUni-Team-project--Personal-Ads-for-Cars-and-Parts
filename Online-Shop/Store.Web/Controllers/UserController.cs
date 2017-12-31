namespace Store.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.AddressViewModels;
    using Store.Services.Models.ProductViewModels;
    using Store.Web.Models.UserViewModels;
    using System.Threading.Tasks;

    public class UserController : Controller
    {
        private IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;

        public UserController(UserManager<User> userManager,
            IUserService userService,
            IProductService productService)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.productService = productService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult SetAddress() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetAddress(AddressViewModel model)
        {
            var user = await this.userManager.GetUserAsync(User);
            this.userService.SetAddress(user, model);

            return RedirectToAction("Details");
        }

        public async Task<IActionResult> ProductsForSale(string sellerId = null)
        {
            if (sellerId == null)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return BadRequest();
                }

                sellerId = this.userManager.GetUserId(User);
            }

            var mappedProducts = this.productService.ProductsBySeller(sellerId);
            var seller = await this.userManager.FindByIdAsync(sellerId);

            var productsToShow = new UserProductsListViewModel
            {
                SellerId = seller.Id,
                SellerUserName = seller.UserName,
                IsRequestSenderOwner = seller.Id == sellerId,
                ProductsToSell = mappedProducts
            };

            return View(nameof(ProductsForSale), productsToShow);
        }

        public IActionResult Details(string userId = null)
        {
            if (userId == null)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return BadRequest();
                }

                userId = this.userManager.GetUserId(User);
            }

            var model = this.userService.GetUserDetailsModel(userId);

            return View(model);
        }

    }
}
