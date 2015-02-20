using com.azi.Compressor;
using com.azi.image;

namespace com.azi.Filters
{
    public interface IColorMap16ToRgb8Filter : IFilter
    {
        RgbImageFile Process(ColorImageFile<ushort> image);
    }

    public class ColorMap16ToRgb8CompressorFilter : IColorMap16ToRgb8Filter
    {
        public ICompressor Compressor { set; get; }

        public RgbImageFile Process(ColorImageFile<ushort> image)
        {
            return new RgbImageFile
            {
                Exif = image.Exif,
                Pixels = Compressor.Compress(image.Pixels, 4)
            };
        }
    }
}
