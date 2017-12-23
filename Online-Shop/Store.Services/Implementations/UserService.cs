namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.AddressViewModels;
    using Store.Services.Models.UserViewModels;
    using System.Linq;

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

        public void SetAddress(User user, AddressViewModel model)
        {
            var address = this.addressService.GetAddress(model);
            user.Address = address;
            this.db.SaveChanges();
        }

        public UserDetailsViewModel GetUserDetailsModel(string userId)
        {
            var model = this.db.Users
                .Select(u => new UserDetailsViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    SoldItemsCount = u.SoldInvoices
                        .Select(i => i.InvoiceProducts
                            .Select(ip => ip.Quantity)
                            .Sum())
                        .Sum(), 
                    Address = new AddressViewModel
                    {
                        Street = u.Address.Street, 
                        TownName = u.Address.Town.Name, 
                        PostCode = u.Address.Town.PostCode, 
                        CountryName = u.Address.Town.Country.Name
                    }
                })
                .FirstOrDefault();

            return model;
        }
    }
}
