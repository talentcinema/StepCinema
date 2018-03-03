using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class StudiesGridModel
    {
        public StudiesGridArgumentModel Arguments { get; set; }
        public StudiesGridColumnModel Columns { get; set; }
        public List<StudiesGridValueModel> Values { get; set; }
    }
    public class StudiesGridArgumentModel
    {
        public StudiesGridFilterModel Filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class StudiesGridFilterModel
    {
        public string StudyNumber { get; set; }
        public string StudyName { get; set; }
        public string Active { get; set; }
    }
    public class StudiesGridColumnModel
    {
        public GridColumnItemModel StudyNumber { get; set; }
        public GridColumnItemModel StudyName { get; set; }
        public GridColumnItemModel NoOfPeriods { get; set; }
        public GridColumnItemModel CreatedOn { get; set; }
        public GridColumnItemModel Satus { get; set; }
        public GridColumnItemModel Active { get; set; }
        public static StudiesGridColumnModel GetColumns()
        {
            var cols = new StudiesGridColumnModel();
            cols.StudyNumber = new GridColumnItemModel() { Name = "Study Number", Sortable = true, Width = "20%", Align = "left" };
            cols.StudyName = new GridColumnItemModel() { Name = "Study Name", Sortable = true, Width = "20%" };
            cols.NoOfPeriods = new GridColumnItemModel() { Name = "No. of Periods", Sortable = true, Width = "10%" };
            cols.CreatedOn = new GridColumnItemModel() { Name = "Created On", Sortable = true, Width = "15%" };
            cols.Satus = new GridColumnItemModel() { Name = "Status", Sortable = true, Width = "10%" };
            cols.Active = new GridColumnItemModel() { Name = "Active", Sortable = true, Width = "10%" };
            return cols;
        }
    }
    public class StudiesGridValueModel
    {
        public int StudyId { get; set; }
        public string StudyNumber { get; set; }
        public string StudyName { get; set; }
        public int NoOfPeriods { get; set; }
        public string CreatedOn { get; set; }
        public string Satus { get; set; }
        public string Active { get; set; }
    }
}
