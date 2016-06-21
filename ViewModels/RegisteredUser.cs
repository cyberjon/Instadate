using System.ComponentModel.DataAnnotations;

namespace WebApplication9.ViewModels
{
    public class RegisteredUser
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "This is not a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^.*[a-zA-Z]+.*$",
        ErrorMessage = "Password should contain alphabet letters.")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password should between 6-8 characters.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
