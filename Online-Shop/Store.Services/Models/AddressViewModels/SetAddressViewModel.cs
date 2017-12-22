namespace Store.Services.Models.AddressViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static Store.Data.ModelConstants;

    public class SetAddressViewModel
    {
        [Required]
        [MinLength(CountryNameMinLength)]
        [MaxLength(CountryNameMaxLength)]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Required]
        [MinLength(TownNameMinLength)]
        [MaxLength(TownNameMaxLength)]
        [Display(Name = "Town")]
        public string TownName { get; set; }

        [MaxLength(TownPostCodeMaxLength)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [MinLength(AddressStreetMinLength)]
        [MaxLength(AddressStreetMaxLength)]
        public string Street { get; set; }
    }
}
