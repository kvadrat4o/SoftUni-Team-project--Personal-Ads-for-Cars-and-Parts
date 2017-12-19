using System.Security.Claims;
using Store.Data.Models;
using Store.Services.Models;

namespace Store.Services.Interfaces
{
    public interface IUserService
    {
        void SetAddress(User user, SetAddressViewModel model);
    }
}
