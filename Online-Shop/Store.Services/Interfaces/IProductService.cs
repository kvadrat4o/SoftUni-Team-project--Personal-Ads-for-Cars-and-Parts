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

        Task<TModel> GetProductAsync<TModel>(int id, string sellerId);

        Task<TModel> GetProductAsync<TModel>(int id);

        Task<Product> GetProductAsync(int id);

        Task<Product> GetProductAsync(int id, string sellerId);

        Task<Product> GetProductAsync(string title);

        Task<Product> Edit(EditProductViewModel newProductData, string requestUserId);

        ProductDetailsViewModel[] ProductsBySeller(string sellerId);

        List<Product> AllProductsForSale();

        List<TModel> ProductsByCategory<TModel>(Category category);

        Paginator<ListOrderedProductViewModel[]> GetOrderedProducts(string buyerId, int page);
    }
}
