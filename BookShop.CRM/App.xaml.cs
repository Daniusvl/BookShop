using Autofac;
using System.Windows;

namespace BookShop.CRM
{
    public partial class App : Application
    {
        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            IContainer container = AutoFac.BuildDI();
            MainWindow window = container.Resolve<MainWindow>();

            window.Show();
        }
    }
}
