namespace Store.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Town
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TownNameMinLength)]
        [MaxLength(TownNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(TownPostCodeMaxLength)]
        public string PostCode { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
    }
}
