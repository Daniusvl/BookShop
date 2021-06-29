using Autofac;
using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;

namespace BookShop.CRM
{
    public static class AutoFac
    {
        public static IContainer BuildDI()
        {
            ContainerBuilder builder = new();

            // Core
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserManager>().As<IUserManager>();

            //ViewModels

            //Views
            builder.RegisterType<MainWindow>().AsSelf();

            return builder.Build();
        }
    }
}
