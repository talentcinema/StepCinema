using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StepCinemaModels.Models
{
    public class TSStatusModel
    {
        [Required(ErrorMessage = "Detail required")]
        public int DetailId { get; set; }

        [Required(ErrorMessage = "Study Number required")]
        public string StudyNumber { get; set; }

        [Required(ErrorMessage = "Subject Number required")]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = "Screening Number required")]
        public string ScreeningNo { get; set; }

        [Required(ErrorMessage = "Registered Number required")]
        public string RegisteredNo { get; set; }

        [Required(ErrorMessage = "Site Id required")]
        public string SiteId { get; set; }

        [Required(ErrorMessage = "Protocol Id required")]
        public string ProtocolTitle { get; set; }

        public int Status { get; set; }
    }

    public class NextPrevPages
    {
        public string NextPage { get; set; }
        public int NextPeriod { get; set; }
        public string PrevPage { get; set; }
        public int PrevPeriod { get; set; }
        public string GetNextUrl(int detail, int study)
        {
            if (string.IsNullOrWhiteSpace(NextPage))
            {
                return string.Format(string.Format("~/CRFForm/End/{0}?study={1}", detail, study));
            }
            return string.Format("~/CRFForm/Page/{0}?period={1}&detail={2}", NextPage, NextPeriod, detail); 
        }
        public string GetPrevUrl(int detail, int study)
        {
            if (string.IsNullOrWhiteSpace(PrevPage))
            {
                return string.Format(string.Format("~/CRFForm/Start/{0}?study={1}", detail, study));
            }
            return string.Format("~/CRFForm/Page/{0}?period={1}&detail={2}", PrevPage, PrevPeriod, detail);
        }
    }

    public class CRFStatusGridModel
    {
        public CRFStatusGridArgumentModel Arguments { get; set; }
        public CRFStatusGridColumnModel Columns { get; set; }
        public List<CRFStatusGridValueModel> Values { get; set; }
    }
    public class CRFStatusGridArgumentModel
    {
        public CRFStatusGridFilterModel filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class CRFStatusGridFilterModel
    {
        public int StudyId { get; set; }
        public string SubjectId { get; set; }
        public int StatusId { get; set; }
    }

    public class CRFStatusGridColumnModel
    {
        public GridColumnItemModel SubjectId { get; set; }
        public GridColumnItemModel StatusName { get; set; }
        public static CRFStatusGridColumnModel GetColumns()
        {
            var cols = new CRFStatusGridColumnModel();
            cols.SubjectId = new GridColumnItemModel() { Name = "Subject Id", Sortable = true, Width = "60%", Align = "left" };
            cols.StatusName = new GridColumnItemModel() { Name = "Status", Sortable = true, Width = "40%" };
            return cols;
        }
    }
    public class CRFStatusGridValueModel
    {
        public int DetailId { get; set; }
        public string SubjectId { get; set; }
        public string StatusName { get; set; }
    }
}


