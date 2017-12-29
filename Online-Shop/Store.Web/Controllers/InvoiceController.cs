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
    }
}
