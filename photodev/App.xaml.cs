using System.IO;
using System.Threading.Tasks;
using System.Windows;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16.Demosaic;
using com.azi.Image;

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
                RawImageFile<ushort> rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                white.WhiteColor = rawimage.Exif.WhiteColor;

                var gamma = new GammaFilter();

                var light = new LightFilter();
                var saturation = new SaturationFilter {Saturation = 1.5f};

                var colorMatrix = new ColorMatrixFilter
                {
                    Matrix = new[,]
                    {
                        {1.87f, -0.81f, -0.06f},
                        {-0.06f, 1.35f, -0.29f},
                        {0.05f, -0.37f, 1.32f}
                    }
                };

                var compressor = new ColorCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    //saturation,
                    colorMatrix,
                    compressor
                });
                pipeline.AutoAdjust(rawimage.Raw, white);

                pipeline.AutoAdjust(rawimage.Raw, light);

                return new RgbImageFile
                {
                    Pixels = pipeline.RawMapToRGB(rawimage.Raw),
                    Exif = rawimage.Exif
                };
            });
        }
    }
}