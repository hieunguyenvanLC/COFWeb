using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.AzureFunctions.Ioc
{
    public class RegistrationHandler
    {
        public Action<ContainerBuilder> RegisterTypeAsInstancePerDependency { get; set; }
    }
}
