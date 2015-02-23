using System;

namespace com.azi.image
{
    public class Rgb8Map
    {
        public delegate void RgbConvertor<T>(Color<T> color, byte[] rgb, int offset, int maxBits) where T : IComparable<T>;

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

        public static void CopyConvertor(Color<byte> pixel, byte[] rgb, int offset, int maxBits)
        {
            rgb[offset + 0] = pixel[0];
            rgb[offset + 1] = pixel[1];
            rgb[offset + 2] = pixel[2];
        }

        public static Rgb8Map ConvertoToRgb<T>(ColorMap<T> map, int strideBytesAlign, RgbConvertor<T> rgbConvertor) where T : IComparable<T>
        {
            var width = map.Width;
            var height = map.Height;
            var stride = strideBytesAlign*(width*BytesPerPixel + strideBytesAlign - 1)/strideBytesAlign;
            var rgb = new byte[height*stride];

            var result = new Rgb8Map(width, height, stride, rgb);

            var pixel = map.GetPixel();
            for (var y = 0; y < height; y++)
            {
                var pos = y*stride;
                for (var x = 0; x < width; x++)
                {
                    rgbConvertor(pixel, rgb, pos, map.MaxBits);
                    pos += BytesPerPixel;
                    pixel.MoveNext();
                }
            }

            return result;
        }
    }
}