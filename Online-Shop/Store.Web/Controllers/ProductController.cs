namespace Store.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
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
                TempData[WebConstants.InfoMessageKey] = "Before create a product you must set your address!";
                return RedirectToAction("SetAddress", "User");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            model.Title = model.Title.Trim();
            var product = Mapper.Map<Product>(model);

            var sellerId = this.userManager.GetUserId(User);
            product.SellerId = sellerId;

            var creationMessage = await this.productService.CreateAsync(product);
            if (creationMessage != null)
            {
                TempData[WebConstants.WarningMessageKey] = creationMessage;
                return View(model);
            }

            TempData[WebConstants.SuccessMessageKey] = $"Product {model.Title} is successfully created.";

            return RedirectToAction("Details", new { title = model.Title });
        }

        [HttpGet]
        public IActionResult Edit() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string title)
        {
            var productToEdit = await this.productService.GetProduct(title);

            return View("Details");
        }

        [Authorize]
        public async Task<IActionResult> Delete (string title)
        {
            var productToDelete = await this.productService.GetProduct(title);
            this.productService.Delete(productToDelete);

            TempData[WebConstants.SuccessMessageKey] = $"Product {title} was succcesfully deleted.";

            return RedirectToAction("AllProducts", "User");
        }

        public async Task<IActionResult> Details(string title)
        {
            var product = await this.productService.GetProduct(title);

            if (product == null)
            {
                return NotFound();
            }

            var mapped = Mapper.Map<DetailsProductViewModel>(product);

            return View(nameof(Details), mapped);
        }
    }
}
