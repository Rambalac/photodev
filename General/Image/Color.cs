using System;

namespace com.azi.image
{
    public class Color<T>
    {
        private readonly ColorMap<T> _map;
        private int _index;
        internal Color(ColorMap<T> map, int x, int y)
        {
            this._map = map;
            _index = (y * map.Width + x) * 3;
        }

        public void Next()
        {
            _index += 3;
        }
        public void Set(T[] values)
        {
            _map.Rgb[_index + 0] = values[0];
            _map.Rgb[_index + 1] = values[1];
            _map.Rgb[_index + 2] = values[2];
        }
        public T[] Get()
        {
            return new T[3] {
                _map.Rgb[_index + 0],
                _map.Rgb[_index + 1],
                _map.Rgb[_index + 2]
            };
        }

        public T this[int i]
        {
            get
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                return _map.Rgb[_index + i];
            }
            set
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                _map.Rgb[_index + i] = value;
            }
        }
    }

    public class ColorMap<T>
    {
        public readonly int Width;
        public readonly int Height;
        public readonly T[] Rgb;
        public readonly int MaxBits;
        public ColorMap(int w, int h, int maxBits)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = new T[w * h * 3];
        }

        public Color<T> GetPixel()
        {
            return GetPixel(0, 0);
        }
        public Color<T> GetPixel(int x, int y)
        {
            return new Color<T>(this, x, y);
        }
    }

    public class Rgb8Map
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int Stride;
        public readonly byte[] Rgb;
        public const int BytesPerPixel = 3;

        public static void CopyConvertor(Color<byte> pixel, byte[] rgb, int offset, int maxBits)
        {
            rgb[offset + 0] = pixel[0];
            rgb[offset + 1] = pixel[1];
            rgb[offset + 2] = pixel[2];
        }

        private Rgb8Map(int width, int height, int stride, byte[] rgb)
        {
            Width = width;
            Height = height;
            Stride = stride;
            Rgb = rgb;
        }

        public delegate void RgbConvertor<T>(Color<T> color, byte[] rgb, int offset, int maxBits);

        public static Rgb8Map ConvertoToRgb<T>(ColorMap<T> map, int strideBytesAlign, RgbConvertor<T> rgbConvertor)
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
                    rgbConvertor(pixel, rgb, pos, map.MaxBits);
                    pos += BytesPerPixel;
                    pixel.Next();
                }
            }

            return result;
        }
    }
}
