using System;

namespace com.azi.Image
{
    public class RGB8Map
    {
        public delegate void RgbConvertor<T>(Color<T> color, byte[] rgb, int rgbOffset) where T : IComparable<T>;

        public const int BytesPerPixel = 3;

        public readonly int Height;
        public readonly byte[] Rgb;
        public readonly int Stride;
        public readonly int Width;

        public RGB8Map(int w, int h)
            : this(w, h, (4 * (w * 3 + 3) / 4), new byte[w * h * 3])
        {

        }

        private RGB8Map(int width, int height, int stride, byte[] rgb)
        {
            Width = width;
            Height = height;
            Stride = stride;
            Rgb = rgb;
        }

        public Color<byte> GetRow(int y)
        {
            return new Color<byte>(this, y);
        }


        public static RGB8Map ConvertoToRgb<T>(ColorMap<T> map, int strideBytesAlign, RgbConvertor<T> rgbConvertor) where T : IComparable<T>
        {
            var width = map.Width;
            var height = map.Height;
            var stride = 4 * (width * 3 + 3) / 4;
            var rgb = new byte[height * stride];

            var result = new RGB8Map(width, height, stride, rgb);

            var pixel = map.GetPixel();
            for (var y = 0; y < height; y++)
            {
                var pos = y * stride;
                for (var x = 0; x < width; x++)
                {
                    rgbConvertor(pixel, rgb, pos);
                    pos += BytesPerPixel;
                    pixel.MoveNext();
                }
            }

            return result;
        }
    }
}