using com.azi.Compressor;
using com.azi.image;

namespace com.azi.Filters.ColorMapToRgb8
{
    public class CompressorFilter : IColorMapToRgb8Filter
    {
        public ICompressor Compressor { set; get; }

        public RgbImageFile Process(ColorImageFile image)
        {
            return new RgbImageFile
            {
                Exif = image.Exif,
                Pixels = Compressor.Compress(image.Pixels, 4)
            };
        }
    }
}