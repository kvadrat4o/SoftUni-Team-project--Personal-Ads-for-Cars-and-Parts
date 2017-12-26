namespace Store.Services.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using System;
    using System.Collections.Generic;
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
            invoice.IsPayed = true;

            await this.db.ShippingRecords.AddRangeAsync(shippingRecords);
            await this.db.SaveChangesAsync();
        }

        private async Task<IEnumerable<ShippingRecord>> CreateShippingRecordAsync(Invoice invoice)
        {
            if (!invoice.IsPayed)
            {
                throw new InvalidOperationException("This invoice is not payed yet!");
            }

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

        private async Task CheckProductQuantityAsync(ProductInvoice ip, Invoice invoice)
        {
            if (ip.Product.Quantity < ip.Quantity)
            {
                var message = string.Empty;
                if (ip.Product.Quantity == 0)
                {
                    message = $"We are sorry. Product {ip.Product.Title} ended and we removed it from your order!";
                    if (invoice.InvoiceProducts.Count > 1)
                    {
                        string.Concat(message, " If you still want to pay for the rest of your items you can do it now.");
                    }

                    this.db.ProductsInvoices.Remove(ip);
                    await this.db.SaveChangesAsync();
                }
                else
                {
                    message = $"Currently the available quantity of {ip.Product.Title} is {ip.Product.Quantity}! We updated the quantity with the maximal possible at this momment. If you still want to pay for the rest of your items you can do it now.";
                    ip.Quantity = ip.Product.Quantity;
                    await this.db.SaveChangesAsync();
                }

                throw new InvalidOperationException(message);
            }
        }
    }
}
