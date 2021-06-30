using BookShop.CRM.ViewModels;
using System.Windows;

namespace BookShop.CRM
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow(AuthenticationViewModel view_model)
        {
            InitializeComponent();
            DataContext = view_model;
        }
    }
}
