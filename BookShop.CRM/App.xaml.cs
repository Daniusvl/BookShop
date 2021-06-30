using Autofac;
using System.Windows;

namespace BookShop.CRM
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IContainer container = AutoFac.BuildDI();
            AuthenticationWindow window = container.Resolve<AuthenticationWindow>();

            window.Show();
        }
    }
}
