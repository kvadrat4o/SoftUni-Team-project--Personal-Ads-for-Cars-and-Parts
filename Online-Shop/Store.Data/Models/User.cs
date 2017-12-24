namespace Store.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static ModelConstants;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(UserAvatarPathMaxLength)]
        public string Avatar { get; set; } = UserDefaultAvatarPath;

        [Range(MoneyMinValue, MoneyMaxValue)]
        public decimal MoneyBalance { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Product> ProductsToSell { get; set; } = new HashSet<Product>();

        public ICollection<Invoice> SoldInvoices { get; set; } = new HashSet<Invoice>();

        public ICollection<Invoice> BoughtInvoices { get; set; } = new HashSet<Invoice>();

        public ICollection<Feedback> SentFeedbacks { get; set; } = new HashSet<Feedback>();

        public ICollection<Feedback> ReceivedFeedbacks { get; set; } = new HashSet<Feedback>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
