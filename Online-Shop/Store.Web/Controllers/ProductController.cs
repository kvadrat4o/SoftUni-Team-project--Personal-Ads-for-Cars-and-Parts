namespace Store.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data.Models;
    using Store.Data.Models.Enums;
    using Store.Services.Interfaces;
    using Store.Services.Models.ProductViewModels;
    using Store.Web.Models.ProductViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IInvoiceService invoiceService;
        private readonly UserManager<User> userManager;

        public ProductController(UserManager<User> userManager,
            IProductService productService,
            IInvoiceService invoiceService)
        {
            this.userManager = userManager;
            this.productService = productService;
            this.invoiceService = invoiceService;
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
            return RedirectToAction(nameof(Details), new { id = product.Id, title = product.Title });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id, string title)
        {
            var requestUserId = this.userManager.GetUserId(User);

            var model = await this.productService.GetProductAsync<EditProductViewModel>(id, requestUserId);
            if (model == null || !model.Title.Equals(title))
            {
                return NotFound();
            }

            return View(model);
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

            var mappedProduct = Mapper.Map<ProductDetailsViewModel>(finalProduct);

            return View(nameof(Details), mappedProduct);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, string title)
        {
            var userId = this.userManager.GetUserId(User);
            var product = await this.productService.GetProductAsync(id, userId);

            if (product == null || !product.Title.Equals(title))
            {
                return NotFound();
            }

            this.productService.Delete(product);
            TempData[WebConstants.SuccessMessageKey] = $"Product {title} was succcesfully deleted.";

            return RedirectToAction("AllProducts", "User");
        }

        public async Task<IActionResult> Details(int id, string title)
        {
            var model = await this.productService.GetProductAsync<ProductDetailsViewModel>(id);
            if (model == null || !model.Title.Equals(title))
            {
                return NotFound();
            }

            return View(nameof(Details), model);
        }

        public IActionResult ProductsForSale()
        {
            var products = this.productService.AllProductsForSale();

            var mapped = new List<CatalogProductViewModel>();

            foreach (var product in products)
            {
                mapped.Add(Mapper.Map<CatalogProductViewModel>(product));
            }

            return View(mapped);
        }

        public IActionResult ProductsByCategory(Category category)
        {
            var products = this.productService.ProductsByCategory<CatalogProductViewModel>(category);
            return View(nameof(ProductsForSale), products);
        }

        public IActionResult SearchProductByTitle(string title)
        {
            var product = this.productService.GetProductAsync(title);

            if (product == null)
            {
                TempData[WebConstants.DangerMessageKey] = "There is no such product.";

                return View();
            }
            return RedirectToAction("Details", product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                TempData[WebConstants.DangerMessageKey] = "Quantity must be greater than 0";
                return RedirectToAction(nameof(Details), new { id = productId });
            }

            var product = await this.productService.GetProductAsync(productId);
            if (product == null)
            {
                return BadRequest();
            }
            else if (!product.IsActive)
            {
                TempData[WebConstants.DangerMessageKey] = "This listing is not active. Please try again later or try find the product from other seller";
                return RedirectToAction(nameof(Details), new { id = productId, title = product.Title });
            }
            else if (product.Quantity == 0)
            {
                TempData[WebConstants.DangerMessageKey] = $"This product ended. We are sorry. You can contact the seller to ask for next charge.";
                return RedirectToAction(nameof(Details), new { id = productId, title = product.Title });
            }
            else if (product.Quantity < quantity)
            {
                TempData[WebConstants.DangerMessageKey] = $"Available quantity is {product.Quantity}. If you need bigger quantity than {product.Quantity} you can contact the seller or search for other sellers with the same product.";
                return RedirectToAction(nameof(Details), new { id = productId, title = product.Title });
            }

            var buyerId = this.userManager.GetUserId(User);
            if (product.SellerId == buyerId)
            {
                TempData[WebConstants.WarningMessageKey] = "You cannot buy your own product";
                return RedirectToAction(nameof(Details), new { id = productId, title = product.Title });
            }

            var buyer = await this.userManager.FindByIdAsync(buyerId);
            if (buyer.MoneyBalance < product.Price * quantity)
            {
                TempData[WebConstants.DangerMessageKey] = "You have not enough money!";
                return RedirectToAction(nameof(Details), new { id = productId, title = product.Title });
            }

            var invoice = await this.invoiceService.CreateInvoiceAsync(buyerId);
            await this.invoiceService.AddProductAsync(product, quantity, invoice);

            return RedirectToAction("Pay", "Invoice", new { id = invoice.Id });
        }

        public async Task<IActionResult> ListOrders(int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);
            var paginator = await this.productService.GetOrderedProducts(userId, page);
            paginator.ActionName = nameof(ListOrders);

            return View(paginator);
        }

        public async Task<IActionResult> SoldItems(int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest();
            }

            var sellerId = this.userManager.GetUserId(User);

            var paginator = await this.productService.GetSoldProducts(sellerId, page);
            paginator.ActionName = nameof(SoldItems);

            return View(paginator);
        }

        public async Task<IActionResult> Dispatch(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var errorMessage = await this.productService.Dispatch(id, userId);
            if (errorMessage != null)
            {
                TempData[WebConstants.DangerMessageKey] = errorMessage;
                return RedirectToAction(nameof(SoldItems));
            }

            return RedirectToAction(nameof(SoldItems));
        }
    }
}
