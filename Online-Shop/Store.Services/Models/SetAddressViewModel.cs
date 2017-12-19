namespace Store.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Store.Data.ModelConstants;

    public class SetAddressViewModel
    {
        [Required]
        [MinLength(CountryNameMinLength)]
        [MaxLength(CountryNameMaxLength)]
        public string CountryName { get; set; }

        [Required]
        [MinLength(TownNameMinLength)]
        [MaxLength(TownNameMaxLength)]
        public string TownName { get; set; }

        [MaxLength(TownPostCodeMaxLength)]
        public string PostCode { get; set; }

        [Required]
        [MinLength(AddressStreetMinLength)]
        [MaxLength(AddressStreetMaxLength)]
        public string Street { get; set; }
    }
}
