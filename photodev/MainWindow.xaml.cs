using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace photodev
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
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
            var bmp = BitmapSource.Create(image.Width, image.Height, 75, 75, PixelFormats.Rgb24, null,
               image.Rgb, image.Stride);
            Save(bmp);
            MainImage.Source = bmp;
        }

        private void Save(BitmapSource bmp)
        {
            var encoder = new JpegBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = 80;

            using (var file = File.OpenWrite("P1350577.jpg"))
            {
                encoder.Save(file);
            }
        }
    }
}