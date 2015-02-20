using com.azi.Compressor;
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
            var debayer = new RawToColorMap16DebayerFilter
            {
                Debayer = new AverageDebayer()
            };
            var color16Image = debayer.Process(rawimage);
            var compressor = new ColorMap16ToRgb8CompressorFilter
            {
                Compressor = new SimpleCompressor()
            };
            var image = compressor.Process(color16Image);
            return BitmapSource.Create(image.Width, image.Height, 75, 75, PixelFormats.Rgb24, null, image.Pixels.Rgb, image.Pixels.Stride);
        }
    }
}
