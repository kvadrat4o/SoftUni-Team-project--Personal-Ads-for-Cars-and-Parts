namespace Store.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Invoice
    {
        public int Id { get; set; }

        public string BuyerId { get; set; }
        public User Buyer { get; set; }

        public decimal TotalValue => this.InvoiceProducts
            .Select(ip => ip.Product.Price * ip.Quantity)
            .Sum();

        public decimal NetValue => this.TotalValue * (1 - ModelConstants.VAT / 100);

        public DateTime IssueDate { get; set; }

        public bool IsPayed { get; set; }

        public ICollection<ProductInvoice> InvoiceProducts { get; set; } = new HashSet<ProductInvoice>();
    }
}
