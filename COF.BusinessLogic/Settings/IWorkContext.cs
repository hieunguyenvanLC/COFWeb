using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Settings
{
    public interface IWorkContext
    {
        string CurrentUserId { get; }
        AppUser CurrentUser { get; }
    }
}
