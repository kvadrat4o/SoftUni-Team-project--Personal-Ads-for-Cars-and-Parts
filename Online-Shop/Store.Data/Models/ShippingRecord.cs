namespace Store.Data.Models
{
    using System;

    public class ShippingRecord
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public DateTime? DispatchDate { get; set; }

        public int Quantity { get; set; }
    }
}
