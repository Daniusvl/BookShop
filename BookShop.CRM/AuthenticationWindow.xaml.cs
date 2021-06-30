using BookShop.CRM.ViewModels;
using System.Windows;

namespace BookShop.CRM
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow(AuthenticationViewModel view_model)
        {
            InitializeComponent();
            ViewModel = view_model;
            ViewModel.AuthenticationWindow = this;
            DataContext = ViewModel;
        }

        public AuthenticationViewModel ViewModel { get; }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           await ViewModel.Load();
        }
    }
}
