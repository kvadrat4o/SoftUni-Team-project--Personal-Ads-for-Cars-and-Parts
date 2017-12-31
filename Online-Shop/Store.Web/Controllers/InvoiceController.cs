namespace Store.Web.Controllers
{
    using System;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Web.Models.InvoiceViewModels;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Store.Web.Models;

    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IInvoiceService invoiceService;
        private readonly IProductService productService;

        public InvoiceController(UserManager<User> userManager,
            IInvoiceService invoiceService,
            IProductService productService)
        {
            this.userManager = userManager;
            this.invoiceService = invoiceService;
            this.productService = productService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var invoice = await this.invoiceService.GetInvoiceWithNavPropsAsync(id);
            if (invoice == null)
            {
                TempData[WebConstants.DangerMessageKey] = "This invoice does not exists!";
                return RedirectToAction("Index", "Home");
            }

            if (!invoice.IsPayed)
            {
                try
                {
                    foreach (var ip in invoice.InvoiceProducts)
                    {
                        await this.invoiceService.CheckProductQuantityAsync(ip, invoice.Id);
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    TempData[WebConstants.DangerMessageKey] = ioe.Message;
                }
            }

            var model = Mapper.Map<InvoiceViewModel>(invoice);

            return View(model);
        }

        public async Task<IActionResult> Pay(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var invoice = await this.invoiceService.GetInvoiceWithNavPropsAsync(id);
            if (invoice.BuyerId != userId)
            {
                return BadRequest();
            }

            try
            {
                await this.invoiceService.PayInvoiceAsync(id);
            }
            catch (InvalidOperationException ioe)
            {
                TempData[WebConstants.DangerMessageKey] = ioe.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData[WebConstants.SuccessMessageKey] = WebConstants.OrderCompletedMessageText;
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> RemoveProduct(int productId, int invoiceId)
        {
            if (await this.invoiceService.IsPayedAsync(invoiceId))
            {
                TempData[WebConstants.DangerMessageKey] = "You can not remove already ordered products";
                return RedirectToAction(nameof(Details), new { id = invoiceId });
            }

            var userId = this.userManager.GetUserId(User);
            if (!await this.invoiceService.IsInvoiceCreator(invoiceId, userId))
            {
                return BadRequest();
            }

            var invoiceProduct = await this.invoiceService.GetInvoiceProductAsync(productId, invoiceId);
            if (invoiceProduct == null)
            {
                TempData[WebConstants.DangerMessageKey] = "This invoice doesn't contain such a product!";
                return RedirectToAction(nameof(Details), new { id = invoiceId });
            }

            var remainedProductsCount = await this.invoiceService.RemoveProductFromInvoiceAsync(invoiceProduct);
            if (remainedProductsCount == 0)
            {
                await this.invoiceService.RemoveInvoiceAsync(invoiceId);
                TempData[WebConstants.SuccessMessageKey] = "The product was removed from your order.";
                return RedirectToAction("Index", "Home");
            }

            TempData[WebConstants.SuccessMessageKey] = "The product was removed from your order. Now you can proceed to pay or continue to customize your order.";
            return RedirectToAction(nameof(Details), new { id = invoiceId });
        }

        public async Task<IActionResult> AddProduct(int productId, int quantity)
        {
            var product = await this.productService.GetProductAsync(productId);
            if (product == null)
            {
                TempData[WebConstants.DangerMessageKey] = "Thes product does not exists";
                return RedirectToAction("Index", "Home");
            }
            else if (product.Quantity == 0)
            {
                TempData[WebConstants.DangerMessageKey] = "This product ended! You can search for it from another seller.";
                return RedirectToAction("ProductsByCategory", "Product", new { category = product.Category });
            }
            else if (quantity <= 0)
            {
                TempData[WebConstants.DangerMessageKey] = "Quantity must be greater than zero!";
                return RedirectToAction("Details", "Product", new { id = productId, title = product.Title });
            }
            else if (product.Quantity < quantity)
            {
                TempData[WebConstants.DangerMessageKey] = $"Currently the maximum quantity is {product.Quantity}! If you need more, you can contact the seller to ask for new charge.";
                return RedirectToAction("Details", "Product", new { id = productId, title = product.Title });
            }

            var userId = this.userManager.GetUserId(User);
            var invoice = await this.invoiceService.AddProductAsync(product, userId, quantity);

            return RedirectToAction(nameof(Details), new { id = invoice.Id });
        }

        [Authorize]
        public IActionResult ListOrders(int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);
            var invoices = this.invoiceService.GetInvoicesByBuyer(userId, page);
            var orders = Mapper.Map<ListOrdersViewModel[]>(invoices);

            var model = new Paginator<ListOrdersViewModel[]>
            {
                PageTitle = "Bought Items", 
                Model = orders, 
                CurrentPage = page, 
                AllPages = 5 // TODO -> Set this in Services
            };

            return View("List", model);
        }
    }
}
