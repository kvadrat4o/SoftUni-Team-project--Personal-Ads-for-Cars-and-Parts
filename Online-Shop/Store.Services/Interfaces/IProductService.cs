namespace Store.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Store.Data.Models;
    using Store.Services.Models.ProductViewModels;

    public interface IProductService
    {
        Task<string> CreateAsync(Product product);

        void Delete(Product product);
        
        Task<Product> GetProduct(int id);

        Task<Product> Edit(EditProductViewModel newProductData, string requestUserId);

        IEnumerable<Product> ProductsBySeller(string sellerId);

        Task<Invoice> CreateInvoiceAsync(Product product, int quantity, string buyerId);

        Task<bool> TryPayInvoiceAsync(Invoice invoice);

        Task<Invoice> GetInvoice(int id);
    }
}
