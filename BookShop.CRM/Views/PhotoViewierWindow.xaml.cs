using System.Windows;
using System.Windows.Media.Imaging;

namespace BookShop.CRM
{
    public partial class PhotoViewierWindow : Window
    {
        public PhotoViewierWindow(BitmapImage bitmapImage)
        {
            InitializeComponent();
            image.Source = bitmapImage;
        }
    }
}
