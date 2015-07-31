using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

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
            var bmp = App.MakeBitmap(image.Pixels);
            Save(bmp);
            MainImage.Source = bmp;

            var histbmp = App.MakeBitmap(image.Pixels.GetHistogram().MakeRGB8Map(200, 100));
            HistogramImage.Source = histbmp;
        }

        private static void Save(BitmapSource bmp, string path = null)
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

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "Photo"; // Default file name
            dlg.DefaultExt = ".rw2"; // Default file extension
            dlg.Filter = "Panasonic RAW|*.rw2"; // Filter files by extension 
            dlg.Multiselect = false;

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true) return;

            var image = await App.OpenFile(Path.GetFullPath(dlg.FileName));

            var bmp = App.MakeBitmap(image.Pixels);
            Save(bmp, Path.ChangeExtension(dlg.FileName, ".jpg"));
            MainImage.Source = bmp;

            File.WriteAllBytes(Path.ChangeExtension(dlg.FileName, ".preview.jpg"), image.Exif.Thumbnail);
        }
    }
}