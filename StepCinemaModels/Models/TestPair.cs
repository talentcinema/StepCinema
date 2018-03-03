using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels.Models
{
    public class TestPair<T1, T2, T3>
    {
        public T1 Form { get; set; }
        public List<T2> FieldList { get; set; }
        public List<List<T3>> ValueList { get; set; }
    }
}
