using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class OrderReportInMonth
    {
        public int Day { get; set; }
        public int Total { get; set; }
        public int Cancel { get; set; }
        public int Inprogress { get; set; }
        public int Finish { get; set; }
    }
}
