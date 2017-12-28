namespace Store.Services.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InvoiceService : IInvoiceService
    {
        private readonly StoreDbContext db;

        public InvoiceService(StoreDbContext db)
        {
            this.db = db;
        }

        public async Task<Invoice> CreateInvoiceAsync(string buyerId)
        {
            var invoice = new Invoice { BuyerId = buyerId };

            await this.db.Invoices.AddAsync(invoice);
            await this.db.SaveChangesAsync();

            return invoice;
        }

        public async Task<bool> IsInvoiceCreator(int invoiceId, string userId)
            => await this.db.Invoices
            .FirstOrDefaultAsync(i => i.Id == invoiceId && i.BuyerId == userId) != null;

        public async Task<bool> IsPayedAsync(int invoiceId) 
            => (await this.db.Invoices.FindAsync(invoiceId)).IsPayed;

        public async Task AddProduct(Product product, int quantity, Invoice invoice)
        {
            if (quantity <= 0)
            {
                throw new InvalidOperationException("Quantity cannot be neither less than nor equal to 0");
            }

            invoice.InvoiceProducts.Add(new ProductInvoice
            {
                ProductId = product.Id,
                Quantity = quantity
            });

            await this.db.SaveChangesAsync();
        }

        public async Task<Invoice> GetInvoiceWithNavPropsAsync(int id) => await this.db.Invoices
            .Include(i => i.Buyer)
                .ThenInclude(b => b.Address)
                    .ThenInclude(a => a.Town)
                    .ThenInclude(a => a.Country)
            .Include(i => i.InvoiceProducts)
                .ThenInclude(ip => ip.Product)
            .FirstOrDefaultAsync(i => i.Id == id);

        public async Task PayInvoiceAsync(int invoiceId)
        {
            var invoice = await this.db.Invoices.FindAsync(invoiceId);
                //.Include(i => i.InvoiceProducts)
                //    .ThenInclude(ip => ip.Product)
                //.FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice.IsPayed)
            {
                throw new InvalidOperationException("This invoice is already payed");
            }

            var buyer = await this.db.Users.FindAsync(invoice.BuyerId);
            if (buyer.MoneyBalance < invoice.TotalValue)
            {
                throw new InvalidOperationException("You have not enough money!");
            }

            var shippingRecords = await this.CreateShippingRecordAsync(invoice);

            buyer.MoneyBalance -= invoice.TotalValue;
            invoice.IssueDate = DateTime.Now;
            invoice.IsPayed = true;
            await this.db.ShippingRecords.AddRangeAsync(shippingRecords);
            await this.db.SaveChangesAsync();
        }

        private async Task<IEnumerable<ShippingRecord>> CreateShippingRecordAsync(Invoice invoice)
        {
            var shippingRecords = new List<ShippingRecord>();
            foreach (var ip in invoice.InvoiceProducts)
            {
                await this.CheckProductQuantityAsync(ip, invoice);

                shippingRecords.Add(new ShippingRecord
                {
                    Invoice = invoice,
                    ProductId = ip.ProductId,
                    Quantity = ip.Quantity
                });

                ip.Product.Quantity -= ip.Quantity;
            }

            return shippingRecords;
        }

        private async Task CheckProductQuantityAsync(ProductInvoice invoiceProduct, Invoice invoice)
        {
            if (invoiceProduct.Product.Quantity >= invoiceProduct.Quantity)
            {
                return;
            }

            var errorMessage = string.Empty;

            if (invoiceProduct.Product.Quantity == 0)
            {
                errorMessage = $"We are sorry. Product {invoiceProduct.Product.Title} ended and we removed it from your order!";
                if (invoice.InvoiceProducts.Count > 1)
                {
                    string.Concat(errorMessage, " If you still want to pay for the rest of your items you can do it now.");
                }

                this.db.ProductsInvoices.Remove(invoiceProduct);
                await this.db.SaveChangesAsync();
            }
            else
            {
                errorMessage = $"Currently the available quantity of {invoiceProduct.Product.Title} is {invoiceProduct.Product.Quantity}! We updated the quantity with the maximal possible at this momment. If you still want to pay for the rest of your items you can do it now.";
                invoiceProduct.Quantity = invoiceProduct.Product.Quantity;
                await this.db.SaveChangesAsync();
            }

            throw new InvalidOperationException(errorMessage);
        }

        public Task<ProductInvoice> GetInvoiceProductAsync(int productId, int invoiceId)
            => this.db.ProductsInvoices.FindAsync(productId, invoiceId);

        public async Task<int> RemoveProductFromInvoiceAsync(ProductInvoice invoiceProduct)
        {
            this.db.ProductsInvoices.Remove(invoiceProduct);
            await this.db.SaveChangesAsync();

            var remainedProductsCount = this.db.ProductsInvoices
                .Where(pi => pi.InvoiceId == invoiceProduct.InvoiceId)
                .Count();

            return remainedProductsCount;
        }

        public async Task RemoveInvoiceAsync(int invoiceId)
        {
            var invoice = await this.db.Invoices.FindAsync(invoiceId);

            this.db.Invoices.Remove(invoice);
            await this.db.SaveChangesAsync();
        }
    }
}
