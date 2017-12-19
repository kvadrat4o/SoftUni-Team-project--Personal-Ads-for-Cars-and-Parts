namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models;

    public interface IAddressService
    {
        Address GetAddress(SetAddressViewModel model);
    }
}
