namespace Store.Services.Models.UserViewModels
{
    using Models.AddressViewModels;
    using System.ComponentModel.DataAnnotations;

    using static Store.Data.ModelConstants;

    public class UserDetailsViewModel 
    {
        [Required]
        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MinLength(UserUserNameMinLength)]
        [MaxLength(UserUserNameMaxLength)]
        public string Username { get; set; }

        [MaxLength(UserAvatarPathMaxLength)]
        public string Avatar { get; set; }

        [Display(Name = "Sold Items")]
        public int SoldItemsCount { get; set; }

        public AddressViewModel Address { get; set; }
    }
}
