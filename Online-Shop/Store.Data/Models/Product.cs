namespace Store.Data.Models
{
    using Store.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ProductTitleMinLength)]
        [MaxLength(ProductTitleMaxLength)]
        public string Title { get; set; }

        [Range(MoneyMinValue, MoneyMaxValue)]
        public decimal Price { get; set; }

        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; }

        [MinLength(ProductPartNumberMinLength)]
        [MaxLength(ProductPartNumberMaxLength)]
        public string PartNumber { get; set; }

        [MaxLength(ProductPicturePathMaxLength)]
        public string PicturePath { get; set; }

        public bool IsNew { get; set; }

        [Range(ProductQuantityMinValue, ProductQuantityMaxValue)]
        public int Quantity { get; set; }

        public Category Category { get; set; }

        public int Visits { get; set; }

        public int TimesSold { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate => this.StartDate.AddMonths(ProductMonthsLive);

        public bool IsActive => this.EndDate >= DateTime.Now;

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public string SellerId { get; set; }
        public User Seller { get; set; }

        public ICollection<ProductInvoice> ProductInvoices { get; set; } = new HashSet<ProductInvoice>();

        public ICollection<Car> CompatibleCars { get; set; } = new HashSet<Car>();

        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
