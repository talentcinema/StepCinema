using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StepCinemaModels.Models
{
    public class LoginModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required")]
        public string Name { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class ChangeMyPasswordModel
    {
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

        [DisplayName("Reenter New Password")]
        [Required(ErrorMessage = "New Password is required")]
        [Compare("NewPassword", ErrorMessage = "New Password is not same")]
        public string NewPasswordAgain { get; set; }
    }

    public class ForgotPasswordModel
    {
        [DisplayName("New Password")]
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

        [DisplayName("Reenter New Password")]
        [Required(ErrorMessage = "New Password is required")]
        [Compare("NewPassword", ErrorMessage = "New Password is not same")]
        public string NewPasswordAgain { get; set; }
    }

    public class EmailModel
    {
        public string Email { get; set; }
    }
}