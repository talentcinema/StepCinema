using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StepCinemaModels.Models
{
    public class UserEditModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Is Active")]
        public bool Active { get; set; }

        [DisplayName("New Password")]
        public string Password { get; set; }

        [DisplayName("Reenter New Password")]
        [Compare("Password", ErrorMessage = "Password is not same")]
        public string ReenterPassword { get; set; }
        
        [DisplayName("Select Roles")]
        public string[] Roles { get; set; }

        public List<ListPair> AvailableRoles { get; set; }
    }
}
