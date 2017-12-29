namespace Store.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Store.Data.Models;
    using Store.Services.Models.ProductViewModels;
    using Store.Data.Models.Enums;

    public interface IProductService
    {
        Task<string> CreateAsync(Product product);

        void Delete(Product product);

        Task<Product> GetProductAsync(int id);

        Task<Product> GetProductAsync(string title);

        Task<Product> Edit(EditProductViewModel newProductData, string requestUserId);

        IEnumerable<Product> ProductsBySeller(string sellerId);

        List<Product> AllProductsForSale();

        List<Product> ProductsByCategory(Category category);

    }
}
