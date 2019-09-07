using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COF.UserWeb.Controllers.Core
{
    public class WorkContext : IWorkContext
    {
        public string CurrentUserId => throw new NotImplementedException();

        public AppUser CurrentUser => throw new NotImplementedException();
    }
}