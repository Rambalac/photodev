using System.IO;
using System.Threading.Tasks;
using System.Windows;
using com.azi.Compressor;
using com.azi.Debayer;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16;
using com.azi.image;

namespace photodev
{
    /// <summary>
    ///     Interaction logic for App.xaml
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
                    Debayer = new AverageBGGRDebayer()
                };
                var color16Image = debayer.Process(rawimage);

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                color16Image = white.Process(color16Image);

                var light = new LightFilter();
                //light.AutoAdjust(color16Image);
                light.Multiplier = 2;
                //light.Gamma = 0.3;
                color16Image = light.Process(color16Image);

                var compressor = new CompressorFilter
                {
                    Compressor = new SimpleCompressor()
                };
                return compressor.Process(color16Image);
            });
        }
    }
}