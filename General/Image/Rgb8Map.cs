using System;

namespace com.azi.image
{
    public class Rgb8Map
    {
        public delegate void RgbConvertor(Color color, int[,] curve, byte[] rgb, int offset, int maxBits);

        public const int BytesPerPixel = 3;

        public readonly int Height;
        public readonly byte[] Rgb;
        public readonly int Stride;
        public readonly int Width;

        private Rgb8Map(int width, int height, int stride, byte[] rgb)
        {
            Width = width;
            Height = height;
            Stride = stride;
            Rgb = rgb;
        }

        public static Rgb8Map ConvertoToRgb(ColorMap map, int strideBytesAlign, RgbConvertor rgbConvertor)
        {
            var width = map.Width;
            var height = map.Height;
            var stride = strideBytesAlign * (width * BytesPerPixel + strideBytesAlign - 1) / strideBytesAlign;
            var rgb = new byte[height * stride];

            var result = new Rgb8Map(width, height, stride, rgb);

            var pixel = map.GetPixel();
            for (var y = 0; y < height; y++)
            {
                var pos = y * stride;
                for (var x = 0; x < width; x++)
                {
                    rgbConvertor(pixel, map.Curve, rgb, pos, map.MaxBits);
                    pos += BytesPerPixel;
                    pixel.MoveNext();
                }
            }

            return result;
        }
    }
}