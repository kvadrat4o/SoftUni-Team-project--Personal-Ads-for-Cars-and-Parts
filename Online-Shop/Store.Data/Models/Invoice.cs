namespace Store.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Invoice
    {
        public int Id { get; set; }

        public string SellerId { get; set; }
        public User Seller { get; set; }

        public string BuyerId { get; set; }
        public User Buyer { get; set; }

        public decimal NetValue => this.InvoiceProducts
            .Select(ip => ip.Product.Price * ip.Quantity)
            .Sum();

        public DateTime IssueDate { get; set; }

        public ICollection<ProductInvoice> InvoiceProducts { get; set; } = new HashSet<ProductInvoice>();
    }
}
