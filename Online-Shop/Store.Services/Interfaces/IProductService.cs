namespace Store.Services.Interfaces
{
    using System.Security.Claims;
    using Store.Data.Models;

    public interface IProductService
    {
        void Create(Product product);
    }
}
