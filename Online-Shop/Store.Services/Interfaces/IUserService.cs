using System.Security.Claims;
using Store.Data.Models;
using Store.Services.Models;

namespace Store.Services.Interfaces
{
    public interface IUserService
    {
        object SetAddress(User user, SetAddressViewModel model);
    }
}
