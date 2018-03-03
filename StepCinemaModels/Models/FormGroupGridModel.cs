using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class FormGroupGridModel
    {
        public FormGroupGridArgumentModel Arguments { get; set; }
        public FormGroupGridColumnModel Columns { get; set; }
        public List<FormGroupGridValueModel> Values { get; set; }
    }
    public class FormGroupGridArgumentModel
    {
        public FormGroupGridFilterModel Filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class FormGroupGridFilterModel
    {
        public string FormGroupId { get; set; }
        public string FormGroupName { get; set; }
        public string Active { get; set; }
    }
    public class FormGroupGridColumnModel
    {
        public GridColumnItemModel FormGroupId { get; set; }
        public GridColumnItemModel FormGroupName { get; set; }
        public GridColumnItemModel Active { get; set; }
        public static FormGroupGridColumnModel GetColumns()
        {
            var cols = new FormGroupGridColumnModel();
            cols.FormGroupId = new GridColumnItemModel() { Name = "FormGroup Id", Sortable = true, Width = "30%", Align = "left" };
            cols.FormGroupName = new GridColumnItemModel() { Name = "FormGroup Name", Sortable = true, Width = "25%" };
            cols.Active = new GridColumnItemModel() { Name = "Active", Sortable = true, Width = "20%" };
            return cols;
        }
    }
    public class FormGroupGridValueModel
    {
        public string FormGroupId { get; set; }
        public string FormGroupName { get; set; }
        public string Active { get; set; }
    }
}
