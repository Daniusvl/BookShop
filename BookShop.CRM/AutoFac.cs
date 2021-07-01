using Autofac;
using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;
using BookShop.CRM.ViewModels;

namespace BookShop.CRM
{
    public static class AutoFac
    {
        public static IContainer Container { get; private set; }

        public static IContainer BuildDI()
        {
            ContainerBuilder builder = new();

            // Core
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserManager>().As<IUserManager>();

            //ViewModels
            builder.RegisterType<AuthenticationViewModel>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            //Views
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<AuthenticationWindow>().AsSelf();

            Container = builder.Build();
            return Container;
        }
    }
}
