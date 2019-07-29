using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime ParticipationDate { get; set; }
        public PartnerStatus Status { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }

    public enum PartnerStatus
    {
        Inactive = 0,
        Active = 1
    }
}
