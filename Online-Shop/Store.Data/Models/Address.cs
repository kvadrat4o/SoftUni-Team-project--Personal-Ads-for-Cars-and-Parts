namespace Store.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Address
    {
        public int Id { get; set; }

        [Required]
        [MinLength(AddressStreetMinLength)]
        [MaxLength(AddressStreetMaxLength)]
        public string Street { get; set; }

        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
