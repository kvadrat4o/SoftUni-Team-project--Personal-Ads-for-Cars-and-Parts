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
    using Store.Services.Models.ProductViewModels;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductController : Controller
    {
        private IProductService productService;
        private UserManager<User> userManager;
        private StoreDbContext db;

        public ProductController(UserManager<User> userManager, 
            IProductService productService,
            StoreDbContext db)
        {
            this.userManager = userManager;
            this.productService = productService;
            this.db = db;
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
        public IActionResult Create(CreateProductViewModel model)
        {
            var product = Mapper.Map<Product>(model);

            var sellerId = this.userManager.GetUserId(User);
            product.SellerId = sellerId;
            this.productService.Create(product);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(string title)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Title.Equals(title));

            if (product == null)
            {
                return NotFound();
            }

            var mapped = Mapper.Map<ProductDetailsViewModel>(product);

            return View(nameof(Details), mapped);
        }
    }
}
