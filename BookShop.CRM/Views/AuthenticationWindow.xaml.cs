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
            ViewModel.OpenMainWindow = OpenMainWindow;
            DataContext = ViewModel;
        }
        
        public AuthenticationViewModel ViewModel { get; }

        private void OpenMainWindow()
        {
            IWindowFactory windowFactory = new WindowFactory();
            MainWindow mainWindow = windowFactory.CreateWindow<MainWindow>();
            Close();
            mainWindow.Show();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           await ViewModel.Load();
        }
    }
}
