namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using System.Threading.Tasks;

    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(string buyerId);

        Task AddProduct(Product product, int quantity, Invoice invoice);

        Task<Invoice> GetInvoiceWithNavPropsAsync(int id);

        Task PayInvoiceAsync(int invoiceId);
    }
}
