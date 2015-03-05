using System;
using com.azi.image;

namespace com.azi.Compressor
{
    public class SimpleCompressor : ICompressor
    {
        private static readonly float[,] defaultMatrix =
        {
            {1f,0f,0f},
            {0f,1f,0f},
            {0f,0f,1f}
        };
        public Rgb8Map Compress(ColorMap<ushort> map, int strideBytesAlign)
        {
            var matrix = map.ColorMatrix ?? defaultMatrix;
            var maxVal = map.MaxValue;
            if (map.MaxBits < 8) throw new Exception("MaxBits cannot be less than 8");
            return Rgb8Map.ConvertoToRgb(map, strideBytesAlign,
                (color, curve, rgb, i, bits) =>
                {
                    var rr = curve[0, color[0]];
                    var rg = curve[1, color[1]];
                    var rb = curve[2, color[2]];

                    //var r = (ushort)Math.Min(maxVal, matrix[0, 0] * rr + matrix[0, 1] * rg + matrix[0, 2] * rb);
                    //var g = (ushort)Math.Min(maxVal, matrix[1, 0] * rr + matrix[1, 1] * rg + matrix[1, 2] * rb);
                    //var b = (ushort)Math.Min(maxVal, matrix[2, 0] * rr + matrix[2, 1] * rg + matrix[2, 2] * rb);

                    rgb[i + 0] = (byte)((rr >> (bits - 8)));
                    rgb[i + 1] = (byte)((rg >> (bits - 8)));
                    rgb[i + 2] = (byte)((rb >> (bits - 8)));
                });
        }
    }
}