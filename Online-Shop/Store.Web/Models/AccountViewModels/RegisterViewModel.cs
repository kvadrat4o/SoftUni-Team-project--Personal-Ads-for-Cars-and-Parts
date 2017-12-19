using Store.Data;

namespace Store.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

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

        [Required]
        [MinLength(UserUserNameMinLength)]
        [MaxLength(UserUserNameMaxLength)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
