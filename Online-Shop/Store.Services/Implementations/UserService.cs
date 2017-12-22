namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.AddressViewModels;

    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private StoreDbContext db;
        private IAddressService addressService;

        public UserService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            StoreDbContext db,
            IAddressService addressService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = db;
            this.addressService = addressService;
        }

        public void SetAddress(User user, SetAddressViewModel model)
        {
            var address = this.addressService.GetAddress(model);
            user.Address = address;
            this.db.SaveChanges();
        }
    }
}
