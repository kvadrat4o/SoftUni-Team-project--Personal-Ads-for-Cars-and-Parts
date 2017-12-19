namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Store.Data.Models;
    using Store.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UserService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
    }
}
