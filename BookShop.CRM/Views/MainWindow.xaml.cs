using BookShop.CRM.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BookShop.CRM
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            MainViewModel = mainViewModel;
            MainViewModel.OpenAuthWindow = OpenAuthWindow;
            DataContext = MainViewModel;
        }

        public MainViewModel MainViewModel { get; }

        private void OpenAuthWindow()
        {
            IWindowFactory windowFactory = new WindowFactory();
            AuthenticationWindow authWindow = windowFactory.CreateWindow<AuthenticationWindow>();
            Close();
            authWindow.Show();
        }
    }
}
