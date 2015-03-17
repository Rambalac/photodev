using com.azi.Image;
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
            var bmp = MakeBitmap(image);
            Save(bmp);
            MainImage.Source = bmp;
        }

        private void Save(BitmapSource bmp, string path = null)
        {
            var encoder = new JpegBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = 80;

            using (var file = File.OpenWrite(path ?? "P1350577.jpg"))
            {
                encoder.Save(file);
            }
        }

        WriteableBitmap MakeBitmap(RGB8Map image)
        {
            var bmp = new WriteableBitmap(image.Width, image.Height, 96, 96, PixelFormats.Rgb24, null);
            bmp.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), image.Rgb, image.Stride, 0);
            return bmp;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Photo"; // Default file name
            dlg.DefaultExt = ".rw2"; // Default file extension
            dlg.Filter = "Panasonic RAW|*.rw2"; // Filter files by extension 
            dlg.Multiselect = false;

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                var image = await App.OpenFile(Path.GetFullPath(dlg.FileName));

                var bmp = MakeBitmap(image);
                Save(bmp, Path.ChangeExtension(dlg.FileName, ".jpg"));
                MainImage.Source = bmp;
            }
        }
    }
}