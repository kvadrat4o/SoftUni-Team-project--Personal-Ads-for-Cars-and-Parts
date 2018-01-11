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

        Task<TModel> GetProductAsync<TModel>(string title);

        Task<Product> GetProductAsync(int id);

        Task<Product> GetProductAsync(int id, string sellerId);

        Task<Product> GetProductAsync(string title);

        Task<Product> Edit(EditProductViewModel newProductData, string requestUserId);

        ProductDetailsViewModel[] ProductsBySeller(string sellerId);

        Task<Paginator<CatalogProductViewModel[]>> AllProductsForSale(int page, Category? category);

        List<TModel> ProductsByCategory<TModel>(Category category);

        Task<Paginator<ListOrderedProductViewModel[]>> GetOrderedProducts(string buyerId, int page);

        Task<Paginator<SoldProductViewModel[]>> GetSoldProducts(string sellerId, int page);

        Task<string> Dispatch(int shippingRecordId, string userId);
    }
}
