using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.azi.Image
{
    public class RawBGGRMap<T> : RawMap<T> where T : IComparable<T>
    {
        public RawBGGRMap(int w, int h, int maxBits)
            : base(w, h, maxBits)
        {
        }
    }

    public class RawMap<T> where T : IComparable<T>
    {
        public const int BytesPerPixel = 3;

        public readonly int Height;
        public readonly T[] Raw;
        public readonly int Width;
        public readonly int MaxBits;

        public int MaxValue
        {
            get { return (1 << MaxBits) - 1; }
        }

        public RawMap(int w, int h, int maxBits)
        {
            Width = w;
            Height = h;
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
            return new RawPixel<T>(this, 0, y, Width);
        }
    }
}
