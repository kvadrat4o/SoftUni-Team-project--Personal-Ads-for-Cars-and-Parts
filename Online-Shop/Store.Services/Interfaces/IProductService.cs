namespace Store.Services.Interfaces
{
    using System.Threading.Tasks;
    using Store.Data.Models;
    using Store.Services.Models.ProductViewModels;

    public interface IProductService
    {
        Task<string> CreateAsync(Product product);

        void Delete(Product product);
        
        Task<Product> GetProduct(string title);

        Task<Product> Edit(string oldProductTitle, EditProductViewModel newProductData);
    }
}
