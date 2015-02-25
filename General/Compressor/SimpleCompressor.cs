using System;
using com.azi.image;

namespace com.azi.Compressor
{
    public class SimpleCompressor : ICompressor
    {
        public Rgb8Map Compress(ColorMap<ushort> map, int strideBytesAlign)
        {
            if (map.MaxBits < 8) throw new Exception("MaxBits cannot be less than 8");
            return Rgb8Map.ConvertoToRgb(map, strideBytesAlign, Function);
        }

        private static void Function(Color<ushort> pixel, ushort[,] curve, byte[] rgb, int offset, int maxBits)
        {
            //if (pixel[0] > (1 << maxBits)) throw new Exception("Exceeding value");
            //if (pixel[1] > (1 << maxBits)) throw new Exception("Exceeding value");
            //if (pixel[2] > (1 << maxBits)) throw new Exception("Exceeding value");
            rgb[offset + 0] = (byte)((curve[0, pixel[0]] >> (maxBits - 8)));
            rgb[offset + 1] = (byte)((curve[1, pixel[1]] >> (maxBits - 8)));
            rgb[offset + 2] = (byte)((curve[2, pixel[2]] >> (maxBits - 8)));
        }
    }
}