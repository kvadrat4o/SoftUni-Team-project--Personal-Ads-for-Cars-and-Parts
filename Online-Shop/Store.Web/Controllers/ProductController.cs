namespace Store.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Web.Models.ProductViewModels;
    using System.Threading.Tasks;

    public class ProductController : Controller
    {
        private IProductService productService;
        private UserManager<User> userManager;

        public ProductController(UserManager<User> userManager, 
            IProductService productService)
        {
            this.userManager = userManager;
            this.productService = productService;
        }
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user.AddressId == null)
            {
                return RedirectToAction("SetAddress", "User");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = Mapper.Map<Product>(model);

            var sellerId = this.userManager.GetUserId(User);
            product.SellerId = sellerId;
            this.productService.Create(product);

            return RedirectToAction("Index", "Home");
        }
    }
}
