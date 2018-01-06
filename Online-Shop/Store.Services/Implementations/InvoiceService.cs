namespace Store.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using Store.Services.Models.InvoiceViewModels;
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

        public async Task AddProductAsync(Product product, int quantity, Invoice invoice)
        {
            if (quantity <= 0)
            {
                throw new InvalidOperationException("Quantity cannot be neither less than nor equal to 0");
            }

            var invoiceProduct = invoice.InvoiceProducts.FirstOrDefault(ip => ip.Product.Id == product.Id);
            if (invoiceProduct == null)
            {
                invoice.InvoiceProducts.Add(new ProductInvoice
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    Quantity = quantity
                });
            }
            else
            {
                invoiceProduct.Quantity += quantity;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<InvoiceViewModel> GetInvoiceAsync(int id) => await this.db.Invoices
            .ProjectTo<InvoiceViewModel>()
            .FirstOrDefaultAsync(i => i.Id == id);

        public async Task PayInvoiceAsync(int invoiceId)
        {
            var invoice = await this.db.Invoices
                .Include(i => i.Buyer)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice.IsPayed)
            {
                throw new InvalidOperationException("This invoice is already payed");
            }
            else if (invoice.Buyer.MoneyBalance < invoice.TotalValue)
            {
                throw new InvalidOperationException("You have not enough money!");
            }

            var shippingRecords = await this.CreateShippingRecordAsync(invoice);

            foreach (var ip in invoice.InvoiceProducts)
            {
                ip.Product.Quantity -= ip.Quantity;
            }

            invoice.Buyer.MoneyBalance -= invoice.TotalValue;
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
                await this.CheckProductQuantityAsync(ip, invoice.InvoiceProducts.Count);

                shippingRecords.Add(new ShippingRecord
                {
                    Invoice = invoice,
                    ProductId = ip.ProductId,
                    Quantity = ip.Quantity
                });
            }

            return shippingRecords;
        }

        public async Task CheckProductQuantityAsync(ProductInvoice invoiceProduct, int productsCount)
        {
            if (invoiceProduct.Product.Quantity >= invoiceProduct.Quantity)
            {
                return;
            }

            var errorMessage = string.Empty;

            if (invoiceProduct.Product.Quantity == 0)
            {
                errorMessage = $"We are sorry. Product {invoiceProduct.Product.Title} ended and we removed it from your order!";
                if (productsCount > 1)
                {
                    string.Concat(errorMessage, " If you still want to complete the payment, you can pay now.");
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

        public async Task<Invoice> AddProductAsync(Product product, string userId, int quantity)
        {
            var invoice = this.db.Invoices
                .Include(i => i.InvoiceProducts)
                    .ThenInclude(ip => ip.Product)
                .FirstOrDefault(i => i.BuyerId == userId && !i.IsPayed);
            if (invoice == null)
            {
                invoice = await this.CreateInvoiceAsync(userId);
            }

            await this.AddProductAsync(product, quantity, invoice);

            return invoice;
        }

        public Paginator<ListOrdersViewModel[]> GetInvoicesByBuyer(string buyerId, int page)
        {
            var orders = this.db.Invoices
            .Where(i => i.BuyerId == buyerId)
            .ProjectTo<ListOrdersViewModel>()
            .OrderByDescending(i => i.IssueDate)
            .Skip((page - 1) * ServiceConstants.PageSize)
            .Take(ServiceConstants.PageSize)
            .ToArray();

            var paginator = new Paginator<ListOrdersViewModel[]>
            {
                PageTitle = "Bought Items",
                Model = orders,
                CurrentPage = page,
                PagesCount = orders.Length, 
                AllPages = (int)Math.Ceiling(this.db.Invoices
                    .Where(i => i.BuyerId == buyerId)
                    .Count() * 1.0 / ServiceConstants.PageSize)
            };

            return paginator;
        }
    }
}
