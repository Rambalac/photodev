using System.IO;
using System.Threading.Tasks;
using System.Windows;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16;
using com.azi.Filters.RawToColorMap16.Demosaic;
using com.azi.Image;

namespace photodev
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static async Task<RGB8Map> OpenFile(string p)
        {
            return await Task.Run(() =>
            {
                Stream stream = new FileStream(p, FileMode.Open, FileAccess.Read);
                var rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                //white.WhiteColor = rawimage.Exif.WhiteColor;
                //white.AutoAdjust(color16Image);

                var gamma = new GammaFilter();

                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new RGBCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[] {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.AutoAjust(rawimage.Raw, white);
                pipeline.AutoAjust(rawimage.Raw, light);

                return pipeline.RawMapToRGB(rawimage.Raw);
            });
        }
    }
}