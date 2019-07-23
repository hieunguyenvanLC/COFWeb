using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface ILogic
    {
        int? AccountId { get; set; }
    }
    public class BaseService : ILogic
    {
        public int? AccountId { get; set; }
    }
}
