using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Infrastructure
{
    public interface IPartnerContext
    {
        int PartnerId { get; set; }
    }
    public class PartnerContext : IPartnerContext
    {
        public int PartnerId { get; set; }
    }
}
