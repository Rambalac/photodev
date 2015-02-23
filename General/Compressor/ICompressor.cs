using com.azi.image;

namespace com.azi.Compressor
{
    public interface ICompressor
    {
        Rgb8Map Compress(ColorMap<ushort> image, int strideBytesAlign);
    }
}