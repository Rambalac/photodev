using com.azi.Debayer;
using com.azi.decoder.panasonic.rw2;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using com.azi.Filters;

namespace photodev
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static BitmapSource OpenFile(string p)
        {
            Stream stream = new FileStream(p, FileMode.Open, FileAccess.Read);
            var rawimage = new PanasonicRW2Decoder().Decode(stream);
            var debayer = new RawToRgbFilter
            {
                Debayer = new AverageDebayer()
            };
            var image = debayer.Process(rawimage);
            return BitmapSource.Create(image.Width, image.Height, 75, 75, PixelFormats.Rgb24, null, image.GetBytesRgb8(), image.GetStrideRgb8());
        }
    }
}
