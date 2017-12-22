namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<string> CreateAsync(Product product);

        void Delete(Product product);
        
        Task<Product> GetProduct(string title);
    }
}
