namespace Store.Web.Controllers
{
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
            var invoice = await this.invoiceService.GetInvoiceAsync(id);

            var model = Mapper.Map<InvoiceViewModel>(invoice);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Pay(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var invoice = await this.invoiceService.GetInvoiceAsync(id);
            if (invoice.BuyerId != userId)
            {
                return BadRequest();
            }

            await this.invoiceService.PayInvoiceAsync(id);

            TempData[WebConstants.SuccessMessageKey] = "Thank you for your purchase. Your order will be shipped as soon as possible.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult RemoveProduct(int productId, int invoiceId)
        {
            return null;
        }
    }
}
