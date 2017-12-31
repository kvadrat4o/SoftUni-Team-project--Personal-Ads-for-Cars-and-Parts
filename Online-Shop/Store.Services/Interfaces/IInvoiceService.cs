namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(string buyerId);

        Task<bool> IsInvoiceCreator(int invoiceId, string userId);

        Task<bool> IsPayedAsync(int invoiceId);

        Task AddProductAsync(Product product, int quantity, Invoice invoice);

        Task<Invoice> GetInvoiceWithNavPropsAsync(int id);

        Task PayInvoiceAsync(int invoiceId);

        Task CheckProductQuantityAsync(ProductInvoice invoiceProduct, int productsCount);

        Task<ProductInvoice> GetInvoiceProductAsync(int productId, int invoiceId);

        Task<int> RemoveProductFromInvoiceAsync(ProductInvoice invoiceProduct);

        Task RemoveInvoiceAsync(int invoiceId);

        Task<Invoice> AddProductAsync(Product product, string userId, int quantity);

        Invoice[] GetInvoicesByBuyer(string userId, int page);
    }
}
