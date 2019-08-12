using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class ScheduleTask : BaseEntity
    { 
        public string Name { get; set; }

        public int Seconds { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastSuccessOnUtc { get; set; }
        public DateTime? LastStartedOnUtc { get; set; }

        public bool IsRunning { get; set; }
      
        public string Description { get; set; }
        public int? HourStartOnEst { get; set; }
        public int? HourEndOnEst { get; set; }
    }

    public static class ScheduleTaskName
    {
        public static string TestTask => "A2000 Import Task";
    }
}

