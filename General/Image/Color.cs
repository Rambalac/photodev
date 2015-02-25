using System;
using System.Linq;

namespace com.azi.image
{
    public class Color
    {
        private readonly int _limit;
        private readonly ColorMap _map;
        private int _index;

        internal Color(ColorMap map, int x, int y)
        {
            _map = map;
            _index = (y * map.Width + x) * 3;
            _limit = _map.Height * _map.Width * 3;
        }

        internal Color(ColorMap map, int y)
        {
            _map = map;
            _index = y * map.Width * 3;
            _limit = _index + _map.Width * 3;
        }

        public int R
        {
            get { return _map.Rgb[_index + 0]; }
            set { _map.Rgb[_index + 0] = value; }
        }

        public int G
        {
            get { return _map.Rgb[_index + 1]; }
            set { _map.Rgb[_index + 1] = value; }
        }

        public int B
        {
            get { return _map.Rgb[_index + 2]; }
            set { _map.Rgb[_index + 2] = value; }
        }

        public int[] GetCopy()
        {
            return new[] { R, G, B };
        }

        public int this[int i]
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

        public bool MoveNext()
        {
            _index += 3;
            return _index < _limit;
        }

        public bool SetAndMoveNext(int r, int g, int b)
        {
            _map.Rgb[_index + 0] = r;
            _map.Rgb[_index + 1] = g;
            _map.Rgb[_index + 2] = b;
            _index += 3;
            return _index < _limit;
        }

    }

    public static class ColorExtension
    {
        public static int MaxComponent(this Color c)
        {
            return (c.R > c.G)
                ? (c.R > c.B) ? c.R : c.B
                : (c.G > c.B) ? c.G : c.B;
        }

        public static int Brightness(this Color c)
        {
            return c.R * c.R + c.G * c.G + c.B * c.B;
        }

        public static double BrightnessSqrt(this Color c)
        {
            return Math.Sqrt(c.Brightness());
        }

        public static int Brightness(this int[] c)
        {
            return c[0] * c[0] + c[1] * c[1] + c[2] * c[2];
        }

        public static double BrightnessSqrt(this int[] c)
        {
            return Math.Sqrt(c.Brightness());
        }

        public static double Brightness(this double[] c)
        {
            return c[0] * c[0] + c[1] * c[1] + c[2] * c[2];
        }

        public static double BrightnessSqrt(this double[] c)
        {
            return Math.Sqrt(c.Brightness());
        }

        public static int[] Normalize(this int[] color, int maxBits)
        {
            var colorNorma = color.BrightnessSqrt();

            var d = (double)((1 << maxBits) - 1);
            return color.Select(v => (int)(v * d / colorNorma)).ToArray();
        }
    }
}