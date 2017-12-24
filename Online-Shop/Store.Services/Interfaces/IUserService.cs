namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.AddressViewModels;
    using Store.Services.Models.UserViewModels;
    using System.Threading.Tasks;

    public interface IUserService
    {
        void SetAddress(User user, AddressViewModel model);

        UserDetailsViewModel GetUserDetailsModel(string userId);

        Task SetAvatar(string picturePath, string userId);
    }
}
