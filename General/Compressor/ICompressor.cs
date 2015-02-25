using com.azi.image;

namespace com.azi.Compressor
{
    public interface ICompressor
    {
        Rgb8Map Compress(ColorMap image, int strideBytesAlign);
    }
}