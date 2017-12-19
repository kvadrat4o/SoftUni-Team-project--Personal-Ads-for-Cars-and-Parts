namespace Store.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models;

    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private StoreDbContext db;

        public UserService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            StoreDbContext db)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = db;
        }

        public object SetAddress(User user, SetAddressViewModel model)
        {
            var country = this.db.Countries
                .FirstOrDefault(c => c.Name.Equals(model.CountryName, StringComparison.OrdinalIgnoreCase));
            return null;
        }
    }
}
