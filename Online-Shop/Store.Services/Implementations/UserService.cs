namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.AddressViewModels;
    using Store.Services.Models.UserViewModels;
    using System.Linq;
    using System.Threading.Tasks;

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
                .Where(u => u.Id.Equals(userId))
                .Select(u => new UserDetailsViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Avatar = u.Avatar,
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

        public async Task SetAvatar(string picturePath, string userId)
        {
            var user = this.db.Users.Find(userId);
            user.Avatar = picturePath;
            await this.db.SaveChangesAsync();
        }

        public Task SetPictureAvatar(string picturePath, string userId)
        {
            return null;
        }
    }
}
