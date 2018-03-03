using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class UserGridModel
    {
        public UserGridArgumentModel Arguments { get; set; }
        public UserGridColumnModel Columns { get; set; }
        public List<UserGridValueModel> Values { get; set; }
    }
    public class UserGridArgumentModel
    {
        public UserGridFilterModel Filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class UserGridFilterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Active { get; set; }
    }
    public class UserGridColumnModel
    {
        public GridColumnItemModel Email { get; set; }
        public GridColumnItemModel FirstName { get; set; }
        public GridColumnItemModel LastName { get; set; }
        public GridColumnItemModel Active { get; set; }
        public static UserGridColumnModel GetColumns()
        {
            var cols = new UserGridColumnModel();
            cols.Email = new GridColumnItemModel() { Name = "Email", Sortable = true, Width = "30%", Align="left" };
            cols.FirstName = new GridColumnItemModel() { Name = "First Name", Sortable = true, Width = "25%" };
            cols.LastName = new GridColumnItemModel() { Name = "Last Name", Sortable = true, Width = "25%" };
            cols.Active = new GridColumnItemModel() { Name = "Active", Sortable = true, Width = "20%" };
            return cols;
        }
    }
    public class UserGridValueModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Active { get; set; }
    }
}
