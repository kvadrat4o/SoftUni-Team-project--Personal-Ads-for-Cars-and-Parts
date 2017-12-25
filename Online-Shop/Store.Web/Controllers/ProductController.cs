namespace Store.Web.Controllers
{
    using System;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.ProductViewModels;
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

            return RedirectToAction("Details", new { id = product.Id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await this.productService.GetProduct(id);

            var requestUserId = this.userManager.GetUserId(User);

            var mappedProduct = Mapper.Map<EditProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            Product finalProduct = null;
            var user = this.userManager.GetUserId(User);

            try
            {
                finalProduct = await this.productService.Edit(model, user);
            }
            catch (InvalidOperationException ioe)
            {
                TempData[WebConstants.DangerMessageKey] = ioe.Message;
                return View(model);
            }

            var mappedProduct = Mapper.Map<DetailsProductViewModel>(finalProduct);

            return View("Details", mappedProduct);
        }

        [Authorize]
        public async Task<IActionResult> Delete (int id)
        {
            var productToDelete = await this.productService.GetProduct(id);

            var userId = this.userManager.GetUserId(User);
            if (productToDelete == null || productToDelete.SellerId != userId)
            {
                return BadRequest();
            }

            var title = productToDelete.Title;

            this.productService.Delete(productToDelete);
            TempData[WebConstants.SuccessMessageKey] = $"Product {productToDelete.Title} was succcesfully deleted.";

            return RedirectToAction("AllProducts", "User");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            var mapped = Mapper.Map<DetailsProductViewModel>(product);

            return View(nameof(Details), mapped);
        }

        [Authorize]
        public IActionResult Buy(string buyerId, int productId, int quantity)
        {
            if (this.userManager.GetUserId(User) != buyerId)
            {
                return RedirectToAction("Details");
            }

            return null;
        }
    }
}
