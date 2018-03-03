using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class StudiesEditModel
    {
        public int StudyId { get; set; }

        [Required(ErrorMessage = "Study Number is required")]
        [DisplayName("Study Number")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Invalid")]
        public string StudyNumber { get; set; }

        [DisplayName("Study Name")]
        [StringLength(500, MinimumLength = 0, ErrorMessage = "Invalid")]
        public string StudyName { get; set; }

        public int NoOfPeriods { get; set; }

        public string SiteId { get; set; }

        public int StudyStatus { get; set; }

        [DisplayName("Is Active")]
        public bool Active { get; set; }

        public int PeriodId { get; set; }

        public List<Periods> PeriodList { get; set; }

        public List<Pages> PageList { get; set; }

        public StudyPageMappingModel studyPageMapping { get; set; }

        public StudiesEditModel()
        {
            PeriodList = new List<Periods>();
            PageList = new List<Pages>();
            studyPageMapping = new StudyPageMappingModel();
        }

        

    }

    public class Periods
    {
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
    }

    public class Pages
    {
        public string PageId { get; set; }
        public string PageName { get; set; }
        public bool Active { get; set; }
        public int PeriodId { get; set; }
    }
}

