using System;
using com.azi.Filters;
using com.azi.image;

namespace com.azi.Compressor
{
    public interface ICompressor
    {
        Rgb8Map Compress(ColorMap<ushort> image, int strideBytesAlign);
    }

    public class SimpleCompressor : ICompressor
    {
        public Rgb8Map Compress(ColorMap<ushort> map, int strideBytesAlign)
        {
            if (map.MaxBits < 8) throw new Exception("MaxBits cannot be less than 8");
            return Rgb8Map.ConvertoToRgb(map, strideBytesAlign, Function);
        }

        private static void Function(Color<ushort> pixel, byte[] rgb, int offset, int maxBits)
        {
            rgb[offset + 0] = (byte)(pixel[0] << (maxBits - 8));
            rgb[offset + 1] = (byte)(pixel[1] << (maxBits - 8));
            rgb[offset + 2] = (byte)(pixel[2] << (maxBits - 8));
        }
    }
}
