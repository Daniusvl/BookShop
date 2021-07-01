using System.Windows;

namespace BookShop.CRM
{
    public interface IWindowFactory
    {
        public TWindow CreateWindow<TWindow>()
            where TWindow : Window;
    }
}
