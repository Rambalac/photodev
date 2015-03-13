using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace com.azi.Image
{
    public class RawPixel<T> where T : IComparable<T>
    {
        private readonly int _limit;
        private readonly RawMap<T> _map;
        private int _index;

        public RawPixel(RawMap<T> map, int x, int y, int limit)
        {
            _map = map;
            _limit = limit;
            _index = y * map.Width + x;
        }

        public RawPixel(RawMap<T> map)
            : this(map, 0, 0, map.Width * map.Height)
        {
        }

        public RawPixel(RawMap<T> map, int x, int y)
            : this(map, x, y, map.Width * map.Height)
        {
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _map.Raw[_index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _map.Raw[_index] = value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetRel(int x, int y)
        {
            return _map.Raw[_index + x + y * _map.Width];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SetRel(int x, int y, T val)
        {
            return _map.Raw[_index + x + y * _map.Width] = val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _index += 1;
            return _index < _limit;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetAndMoveNext(T val)
        {
            _map.Raw[_index] = val;
            return MoveNext();
        }
    }

    public class Color<T> where T : IComparable<T>
    {
        private readonly int _limit;
        private readonly ColorMap<T> _map;
        private int _index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Color(ColorMap<T> map, int x, int y)
        {
            _map = map;
            _index = (y * map.Width + x) * 3;
            _limit = _map.Height * _map.Width * 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Color(ColorMap<T> map, int y)
        {
            _map = map;
            _index = y * map.Width * 3;
            _limit = _index + _map.Width * 3;
        }

        public T R
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _map.Rgb[_index + 0]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _map.Rgb[_index + 0] = value; }
        }

        public T G
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _map.Rgb[_index + 1]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _map.Rgb[_index + 1] = value; }
        }

        public T B
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _map.Rgb[_index + 2]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _map.Rgb[_index + 2] = value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] GetCopy()
        {
            return new[] { R, G, B };
        }

        public T this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                return _map.Rgb[_index + i];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                //if (i > 2) throw new ArgumentException("Should be less than 3");
                _map.Rgb[_index + i] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _index += 3;
            return _index < _limit;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetAndMoveNext(T r, T g, T b)
        {
            _map.Rgb[_index + 0] = r;
            _map.Rgb[_index + 1] = g;
            _map.Rgb[_index + 2] = b;
            return MoveNext();
        }

    }

    public static class ColorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort MaxComponent(this Color<ushort> c)
        {
            return (c.R > c.G)
                ? (c.R > c.B) ? c.R : c.B
                : (c.G > c.B) ? c.G : c.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Brightness(this Color<ushort> c)
        {
            return c.R * c.R + c.G * c.G + c.B * c.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BrightnessSqrt(this Color<ushort> c)
        {
            return Math.Sqrt(c.Brightness());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Brightness(this ushort[] c)
        {
            return c[0] * c[0] + c[1] * c[1] + c[2] * c[2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BrightnessSqrt(this ushort[] c)
        {
            return Math.Sqrt(c.Brightness());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Brightness(this double[] c)
        {
            return c[0] * c[0] + c[1] * c[1] + c[2] * c[2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BrightnessSqrt(this double[] c)
        {
            return Math.Sqrt(c.Brightness());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ToFloat(this ushort[] c, int bits)
        {
            var maxValue = (float)((1 << bits) - 1);
            return c.Select(v => v / maxValue).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] Normalize(this ushort[] color, int maxBits)
        {
            var colorNorma = color.BrightnessSqrt();

            var d = (double)((1 << maxBits) - 1);
            return color.Select(v => (ushort)(v * d / colorNorma)).ToArray();
        }
    }
}