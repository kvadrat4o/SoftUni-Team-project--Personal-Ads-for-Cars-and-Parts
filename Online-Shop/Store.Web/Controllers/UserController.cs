﻿namespace Store.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.AddressViewModels;
    using Store.Web.Models.UserViewModels;
    using Store.Web.Models.ProductViewModels;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserController : Controller
    {
        private IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly StoreDbContext db;


        public UserController(UserManager<User> userManager,
            IUserService userService,
            StoreDbContext db)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.db = db;
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

        [Authorize]
        public async Task<IActionResult> AllProducts()
        {
            var user = await userManager.GetUserAsync(User);
            var products = db.Products.Where(p => p.Seller.UserName.Equals(user.UserName));

            var mappedProducts = Mapper.Map<DetailsProductViewModel[]>(products);
            var productsToShow = new UserProductsListViewModel
            {
                UserName = user.UserName,
                ProductsToSell = mappedProducts
            };

            return View(nameof(AllProducts), productsToShow);
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
