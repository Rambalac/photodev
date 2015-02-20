using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace photodev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainImage_Loaded(object sender, RoutedEventArgs e)
        {
            var image = await App.OpenFile(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2");
            MainImage.Source = BitmapSource.Create(image.Width, image.Height, 75, 75, PixelFormats.Rgb24, null, image.Pixels.Rgb, image.Pixels.Stride);
        }
    }
}
