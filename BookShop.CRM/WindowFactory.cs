using Autofac;
using System.Windows;

namespace BookShop.CRM
{
    public class WindowFactory : IWindowFactory
    {
        public TWindow CreateWindow<TWindow>()
            where TWindow : Window
        {
            return AutoFac.Container.Resolve<TWindow>();
        }
    }
}
