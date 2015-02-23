using System;
using System.Threading.Tasks;
using com.azi.Compressor;
using com.azi.Debayer;
using com.azi.decoder.panasonic.rw2;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using com.azi.Filters;
using com.azi.image;

namespace photodev
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static async Task<RgbImageFile> OpenFile(string p)
        {
            return await Task.Run(() =>
            {
                Stream stream = new FileStream(p, FileMode.Open, FileAccess.Read);
                var rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new DebayerFilter
                {
                    Debayer = new BinningDebayer()
                };
                var color16Image = debayer.Process(rawimage);
                var compressor = new ColorMap16ToRgb8CompressorFilter
                {
                    Compressor = new SimpleCompressor()
                };
                return compressor.Process(color16Image);
            });
        }
    }
}
