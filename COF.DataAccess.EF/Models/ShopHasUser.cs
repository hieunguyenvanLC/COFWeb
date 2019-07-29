using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class ShopHasUser : BaseEntity,IPartner
    {    
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }
        public int PartnerId { get;set; }
    }
}
