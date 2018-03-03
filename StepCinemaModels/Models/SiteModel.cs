using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StepCinemaModels.Models
{
    public class SiteModel
    {
        [DisplayName("Site Id")]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9_\-]+$", ErrorMessage = "Alphabets, number and hyphens(_,-) are only allowed")]
        public string SiteId { get; set; }
        [DisplayName("Site Name")]
        public string SiteName { get; set; }
    }

    public class SiteGridModel
    {
        public SiteGridArgumentModel Arguments { get; set; }
        public SiteGridColumnModel Columns { get; set; }
        public List<SiteGridValueModel> Values { get; set; }
    }
    public class SiteGridArgumentModel
    {
        public SiteGridFilterModel Filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class SiteGridFilterModel
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
    }
    public class SiteGridColumnModel
    {
        public GridColumnItemModel SiteId { get; set; }
        public GridColumnItemModel SiteName { get; set; }
        public static SiteGridColumnModel GetColumns()
        {
            var cols = new SiteGridColumnModel();
            cols.SiteId = new GridColumnItemModel() { Name = "Site Id", Sortable = true, Width = "40%", Align = "left" };
            cols.SiteName = new GridColumnItemModel() { Name = "Site Name", Sortable = true, Width = "60%" };
            return cols;
        }
    }
    public class SiteGridValueModel
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
    }
}


