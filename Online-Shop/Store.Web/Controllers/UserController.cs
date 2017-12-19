namespace Store.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models;
    using System.Threading.Tasks;

    public class UserController : Controller
    {
        private IUserService userService;
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager, 
            IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult SetAddress() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetAddressAsync(SetAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.userManager.GetUserAsync(User);
            var isAddressSet = this.userService.SetAddress(user, model);

            return RedirectToAction("");
        }
    }
}
