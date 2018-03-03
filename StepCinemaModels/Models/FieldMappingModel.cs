using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class FieldMappingModel
    {
        public string StudyNumber { get; set; }
        public string StudyName { get; set; }
        public int StudyId { get; set; }
        public string PageName { get; set; }
        public string PageId { get; set; }
        public int NoOfPeriods { get; set; }
        public string PeriodName { get; set; }
        public int PeriodId { get; set; }
        public string FormName { get; set; }
        public string FormDisable { get; set; }
        public string FormHide { get; set; }
        public int NoOfRows { get; set; }
        public List<FieldMappingFormConfig> FormConfig { get; set; }
        public List<FieldMappingFormField> FormFields { get; set; }
        public List<ListOfValuesModel> DataType { get; set; }
        public FieldMappingModel()
        {
            FormFields = new List<FieldMappingFormField>();
            FormConfig = new List<FieldMappingFormConfig>();
            DataType = new List<ListOfValuesModel>();
        }
    }

    public class FieldMappingFormField
    {
        public bool Disabled { get; set; }
        public bool Display { get; set; }
        public string FieldId { get; set; }
        public string FieldDataType { get; set; }
        public string FieldExportName { get; set; }        
        public string FieldLiteral { get; set; }        
        public string FieldType { get; set; }
        public string FormGroupId { get; set; }
        public string FormId { get; set; }       
        public int PeriodId { get; set; }
        public int StudyId { get; set; }
        public bool FieldMandatory { get; set; }
        public string Unit { get; set; }
        public string LOVId { get; set; }
        public int Order { get; set; }
    }

    public class FieldMappingFormConfig
    {
        public string FormId { get; set; }
        public string FormName { get; set; }
        public int PeriodId { get; set; }
        public string FormGroupId { get; set; }
        public int StudyId { get; set; }
        public int Order { get; set; }
        public string FormType { get; set; }
        public int Rows { get; set; }
        public bool Disabled { get; set; }
        public bool Display { get; set; }
    }
}
