using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace StepCinemaModels.Models
{
    public class GridArgumentModel
    {
        public List<GridFilterPairModel> Filter { get; set; }
        public GridSortModel Sort { get; set; }
        public GridPaginationModel Pagination { get; set; }
    }
    public class GridSortModel
    {
        public string Field { get; set; }
        public string Direction { get; set; } 
    }
    public class GridPaginationModel
    {
        public int MaxPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
    }
    public class GridFilterPairModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class GridColumnItemModel
    {
        public string Name { get; set; }
        public bool Sortable { get; set; }
        public string Width { get; set; }
        public string Align { get; set; }
    }
}
