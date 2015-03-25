using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using com.azi.Image;
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
            RgbImageFile image = await App.OpenFile(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2");
            WriteableBitmap bmp = MakeBitmap(image.Pixels);
            Save(bmp);
            MainImage.Source = bmp;
        }

        private static void Save(BitmapSource bmp, string path = null)
        {
            var encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = 80;

            using (FileStream file = File.OpenWrite(path ?? "P1350577.jpg"))
            {
                encoder.Save(file);
            }
        }

        private static WriteableBitmap MakeBitmap(RGB8Map image)
        {
            var bmp = new WriteableBitmap(image.Width, image.Height, 96, 96, PixelFormats.Rgb24, null);
            bmp.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), image.Rgb, image.Stride, 0);
            return bmp;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "Photo"; // Default file name
            dlg.DefaultExt = ".rw2"; // Default file extension
            dlg.Filter = "Panasonic RAW|*.rw2"; // Filter files by extension 
            dlg.Multiselect = false;

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true) return;

            RgbImageFile image = await App.OpenFile(Path.GetFullPath(dlg.FileName));

            WriteableBitmap bmp = MakeBitmap(image.Pixels);
            Save(bmp, Path.ChangeExtension(dlg.FileName, ".jpg"));
            MainImage.Source = bmp;

            File.WriteAllBytes(Path.ChangeExtension(dlg.FileName, ".preview.jpg"), image.Exif.Thumbnail);
        }
    }
}