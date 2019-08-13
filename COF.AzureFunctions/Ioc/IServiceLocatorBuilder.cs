using Autofac.Core;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.AzureFunctions.Ioc
{
    public interface IServiceLocatorBuilder : IDisposable
    {
        IServiceLocator Build();

        IServiceLocatorBuilder RegisterModule<TModule>(RegistrationHandler handler = null)
            where TModule : IModule, new();
    }
}
