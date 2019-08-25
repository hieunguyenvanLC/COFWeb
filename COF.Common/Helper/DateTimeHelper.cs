using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Common.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime CurentVnTime
        {
            get
            {
                return DateTime.UtcNow.AddHours(7);
            }
        }
    }
}
