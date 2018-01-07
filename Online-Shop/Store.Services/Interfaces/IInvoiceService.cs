namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.InvoiceViewModels;
    using System.Threading.Tasks;

    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(string buyerId);

        Task<bool> IsInvoiceCreator(int invoiceId, string userId);

        Task<bool> IsPayedAsync(int invoiceId);

        Task AddProductAsync(Product product, int quantity, Invoice invoice);

        Task<InvoiceViewModel> GetInvoiceAsync(int id);

        Task PayInvoiceAsync(int invoiceId);

        Task CheckProductQuantityAsync(ProductInvoice invoiceProduct, int productsCount);

        Task<ProductInvoice> GetInvoiceProductAsync(int productId, int invoiceId);

        Task<int> RemoveProductFromInvoiceAsync(ProductInvoice invoiceProduct);

        Task RemoveInvoiceAsync(int invoiceId);

        Task<Invoice> AddProductAsync(Product product, string userId, int quantity);

        Paginator<ListOrderInvoicesViewModel[]> GetInvoicesByBuyer(string userId, int page);

        Task<int?> GetUnpaidInvoiceId(string userId);
    }
}
