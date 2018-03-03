using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class FormGroupEditModel
    {
        [Required(ErrorMessage= "FormGroup Id is required")]
        [DisplayName("FormGroup Id")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Invalid")]
        public string FormGroupId { get; set; }

        [StringLength(200,MinimumLength = 0,ErrorMessage ="Invalid")]
        [DisplayName("FormGroup Name")]
        public string FormGroupName { get; set; }

        [DisplayName("Is Active")]
        public bool Active { get; set; }
    }
}
