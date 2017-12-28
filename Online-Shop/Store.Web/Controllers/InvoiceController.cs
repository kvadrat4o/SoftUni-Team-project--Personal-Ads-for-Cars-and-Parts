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

    public class InvoiceController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IInvoiceService invoiceService;

        public InvoiceController(UserManager<User> userManager, 
            IInvoiceService invoiceService)
        {
            this.userManager = userManager;
            this.invoiceService = invoiceService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var invoice = await this.invoiceService.GetInvoiceWithNavPropsAsync(id);

            var model = Mapper.Map<InvoiceViewModel>(invoice);

            return View(model);
        }

        [Authorize]
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

        public IActionResult RemoveProduct(int productId, int invoiceId)
        {
            return null;
        }
    }
}
