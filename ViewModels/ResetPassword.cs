using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication9.ViewModels
{
    public class ResetPassword
    {
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