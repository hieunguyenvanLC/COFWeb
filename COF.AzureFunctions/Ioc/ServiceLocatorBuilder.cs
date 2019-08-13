using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using COF.AzureFunctions.Helper;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.AzureFunctions.Ioc
{
    public class ServiceLocatorBuilder : IServiceLocatorBuilder
    {
        private ContainerBuilder _containerBuilder;
        private bool _disposed;

        public ServiceLocatorBuilder()
        {
            this._containerBuilder = new ContainerBuilder();
        }

        public IServiceLocator Build()
        {
            var container = this._containerBuilder.Build();

            return new AutofacServiceLocator(container);
        }

        public IServiceLocatorBuilder RegisterModule<TModule>(RegistrationHandler handler = null)
            where TModule : IModule, new()
        {
            this._containerBuilder.RegisterModule<TModule>();

            if (handler.IsNullOrDefault())
            {
                return this;
            }

            if (handler.RegisterTypeAsInstancePerDependency.IsNullOrDefault())
            {
                return this;
            }

            handler.RegisterTypeAsInstancePerDependency.Invoke(this._containerBuilder);

            return this;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.ReleaseManagedResources();
            }

            this.ReleaseUnmanagedResources();

            this._disposed = true;
        }

        protected virtual void ReleaseManagedResources()
        {
            // Release managed resources here.
        }

        protected virtual void ReleaseUnmanagedResources()
        {
            this._containerBuilder = null;
        }
        

    }
}
