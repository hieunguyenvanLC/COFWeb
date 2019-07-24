using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class UserWorkingTime : BaseEntity
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
