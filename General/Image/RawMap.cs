using System;

namespace com.azi.Image
{
    public class RawBGGRMap<T> : RawMap<T>
    {
        public RawBGGRMap(int w, int h, int maxBits)
            : base(w, h, maxBits)
        {
        }
    }

    public class RawMap<T> : IColorMap
    {
        readonly int _height;
        public readonly T[] Raw;
        readonly int _width;
        public readonly int MaxBits;

        public int MaxValue
        {
            get { return (1 << MaxBits) - 1; }
        }

        public RawMap(int w, int h, int maxBits)
        {
            _width = w;
            _height = h;
            MaxBits = maxBits;
            Raw = new T[h * w];
        }

        public RawPixel<T> GetPixel()
        {
            return GetPixel(0, 0);
        }

        public RawPixel<T> GetPixel(int x, int y)
        {
            return new RawPixel<T>(this, x, y);
        }

        public RawPixel<T> GetRow(int y)
        {
            return new RawPixel<T>(this, 0, y, (y + 1) * Width);
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
    }
}
