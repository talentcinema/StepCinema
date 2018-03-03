using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class TSForm
    {
        public string FormId { get; set; }
        public string Name { get; set; }
        public string FormType { get; set; }
        public int Rows { get; set; }
        public Dictionary<string, CRFFieldHead> Head { get; set; }
        public List<Dictionary<string, CRFFieldValue>> Items { get; set; }
        public List<string> FieldOrder { get; set; }
    }
    public class CRFFormPage
    {
        public Dictionary<string, TSForm> FormGroup { get; set; }

        public Dictionary<string, CRFLov> Lovs { get; set; }
    }
    public class CRFFieldHead
    {
        public string FieldId { get; set; }
        public string Text { get; set; }
    }
    public class CRFFieldValue
    {
        public string FieldId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string LovId { get; set; }
        public string Unit { get; set; }
        public string CompareValue { get; set; }
    }
    public class CRFParameters
    {
        public int DetailId { get; set; }
        public int StudyId { get; set; }
        public int PeriodId { get; set; }
        public string FormGroupId { get; set; }
        public string SubjectId { get; set; }
        public string ScreeningNo { get; set; }
        public string RegisteredNo { get; set; }
        public string PageRef { get; set; }
        public int EntryType { get; set; }
        public int CompareEntryType { get; set; }
    }
    public class CRFLovKeyValue
    {
        public string Text { get; set; }
        public string Key { get; set; }
    }
    public class CRFLov
    {
        public List<CRFLovKeyValue> Items { get; set; }
    }
    public class CRFJson
    {
        public string Json { get; set; }
        public CRFParameters Parameters { get; set; }
        public NextPrevPages NavData { get; set; }
    }
    public class CRFFormEndData
    {
        public int StudyId { get; set; }
        public NextPrevPages NavData { get; set; }
        public CRFDetailData Details { get; set; }
    }
    public class CRFDetailData
    {
        public int StudyId { get; set; }
        public int DetailsId { get; set; }
        public string SubjectId { get; set; }
        public int Status { get; set; }
    }
}
