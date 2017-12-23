namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.AddressViewModels;

    public interface IAddressService
    {
        Address GetAddress(AddressViewModel model);
    }
}
