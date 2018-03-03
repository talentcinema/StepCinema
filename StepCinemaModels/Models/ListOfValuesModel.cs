using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class ListOfValuesModel
    {
        public string LOVId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
    }
}
