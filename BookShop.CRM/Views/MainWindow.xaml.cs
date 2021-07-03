using BookShop.CRM.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BookShop.CRM
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            MainViewModel = mainViewModel;
            MainViewModel.OpenAuthWindow = OpenAuthWindow;
            MainViewModel.Photo.ShowPhoto = ShowPhoto;
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

        private void ShowPhoto(List<byte> bytes)
        {
            using MemoryStream memoryStream = new(bytes.ToArray());
            BitmapImage img = new();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.StreamSource = memoryStream;
            img.EndInit();
            PhotoViewierWindow photoViewierWindow = new(img);
            photoViewierWindow.ShowDialog();
        }
    }
}
