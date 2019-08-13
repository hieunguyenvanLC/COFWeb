using Autofac;
using COF.DataAccess.EF;
using COF.DataAccess.EF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.AzureFunctions.Ioc
{
    public class AppModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<EFTransaction>().As<ITransaction>().SingleInstance();
            builder.RegisterType<EFContext>().AsSelf().SingleInstance();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().SingleInstance();
            // Allow inheriting class to register types
            this.Register(builder);
        }

        public virtual void Register(ContainerBuilder containerBuilder)
        {
        }
    }
}
