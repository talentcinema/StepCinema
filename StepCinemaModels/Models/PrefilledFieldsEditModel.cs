using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class PrefilledFieldsEditModel
    {
        public string FormId { get; set; }
        public string FieldId { get; set; }
        public int PeriodId { get; set; }
        public string FormGroupId { get; set; }
        public int StudyId { get; set; }
        public int NoOfRows { get; set; }
        public List<PrefilledFieldsList> ListPrefilledFields { get; set; }
        public List<string> ListFieldId { get; set; }
    }

    public class PrefilledFieldsList
    {
        public string FormId { get; set; }
        public string FieldId { get; set; }
        public int PeriodId { get; set; }
        public string FormGroupId { get; set; }
        public int StudyId { get; set; }
        public int RowIndex { get; set; }
        public string FieldValue { get; set; }        
    }
}
