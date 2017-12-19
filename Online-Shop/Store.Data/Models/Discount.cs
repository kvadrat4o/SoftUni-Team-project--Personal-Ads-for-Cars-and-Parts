namespace Store.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Discount
    {
        public int Id { get; set; }

        public double Percentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDateDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
