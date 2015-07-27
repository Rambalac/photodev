using System.IO;
using System.Threading.Tasks;
using System.Windows;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters;
using com.azi.Filters.VectorMapFilters;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16.Demosaic;
using com.azi.Image;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace photodev
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static WriteableBitmap MakeBitmap(RGB8Map image)
        {
            var bmp = new WriteableBitmap(image.Width, image.Height, 96, 96, PixelFormats.Rgb24, null);
            bmp.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), image.Rgb, image.Stride, 0);
            return bmp;
        }


        internal static async Task<RgbImageFile> OpenFile(string p)
        {
            return await Task.Run(() =>
            {
                Stream stream = new FileStream(p, FileMode.Open, FileAccess.Read);
                var rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                white.WhiteColor = rawimage.Exif.WhiteColor.ToVector3();

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
                    }.ToMatrix4x4()
                };

                var compressor = new VectorCompressorFilter();
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