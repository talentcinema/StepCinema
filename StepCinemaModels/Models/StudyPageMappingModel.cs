using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class StudyPageMappingModel
    {
        public List<string> FormGroupId { get; set; }
        public int PeriodId { get; set; }
        public int Order { get; set; }
        public int StudyId { get; set; }
        public StudyPageMappingModel()
        {
            FormGroupId = new List<string>();
        }
    }
}
