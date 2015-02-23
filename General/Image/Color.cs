using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace com.azi.image
{
    public class Color<T> : IEnumerator<Color<T>> where T : IComparable<T>
    {
        private readonly int _limit;
        private readonly ColorMap<T> _map;
        private int _index;

        internal Color(ColorMap<T> map, int x, int y)
        {
            _map = map;
            _index = (y * map.Width + x) * 3;
            _limit = _map.Height * _map.Width * 3;
        }

        internal Color(ColorMap<T> map, int y)
        {
            _map = map;
            _index = y * map.Width * 3;
            _limit = _index + _map.Width * 3;
        }

        public T R
        {
            get { return _map.Rgb[_index + 0]; }
            set { _map.Rgb[_index + 0] = value; }
        }

        public T G
        {
            get { return _map.Rgb[_index + 1]; }
            set { _map.Rgb[_index + 1] = value; }
        }

        public T B
        {
            get { return _map.Rgb[_index + 2]; }
            set { _map.Rgb[_index + 2] = value; }
        }

        public T[] Get()
        {
            return new[] { R, G, B };
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

        public bool MoveNext()
        {
            _index += 3;
            return _index < _limit;
        }

        public bool SetAndMoveNext(T r, T g, T b)
        {
            _map.Rgb[_index + 0] = r;
            _map.Rgb[_index + 1] = g;
            _map.Rgb[_index + 2] = b;
            _index += 3;
            return _index < _limit;
        }

        public void Reset()
        {
            _index = 0;
        }

        public Color<T> Current
        {
            get { return this; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
        }

        public IEnumerator<Color<T>> GetEnumerator()
        {
            return this;
        }
    }

    public static class ColorExtension
    {
        public static ushort MaxComponent(this Color<ushort> c)
        {
            return (c.R > c.G)
                ? (c.R > c.B) ? c.R : c.B
                : (c.G > c.B) ? c.G : c.B;
        }

        public static double Brightness(this Color<ushort> c)
        {
            return c.R * (double)c.R + c.G * (double)c.G + c.B * (double)c.B;
        }

        public static double BrightnessSqrt(this Color<ushort> c)
        {
            return Math.Sqrt(c.Brightness());
        }

        public static double Brightness(this ushort[] c)
        {
            return c[0] * c[0] + c[1] * c[1] + c[2] * c[2];
        }

        public static double BrightnessSqrt(this ushort[] c)
        {
            return Math.Sqrt(c.Brightness());
        }

        public static ushort[] Normalize(this ushort[] color, int maxBits)
        {
            var maxBitsNormal = new[] { (ushort)(1 << maxBits), (ushort)(1 << maxBits), (ushort)(1 << maxBits) };
            var maxBitsNorma = maxBitsNormal.BrightnessSqrt();
            var colorNorma = color.BrightnessSqrt();
            var enlarge = maxBitsNorma / colorNorma;

            return maxBitsNormal.Select(v => (ushort)(v * enlarge)).ToArray();
        }
    }
}