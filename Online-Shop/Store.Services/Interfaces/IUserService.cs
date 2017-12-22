namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.AddressViewModels;

    public interface IUserService
    {
        void SetAddress(User user, SetAddressViewModel model);
    }
}
