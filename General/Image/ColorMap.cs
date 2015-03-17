using System;
using System.Reflection;

namespace com.azi.Image
{
    public class ColorMap<T> : IColorMap where T : IComparable<T>
    {
        public delegate void ColorMapProcessor(int x, int y, Color<T> input, Color<T> output);

        public delegate T CurveProcessor(int component, int index, T input);

        public readonly int Height;
        public readonly T[] Rgb;
        public readonly int Width;
        public readonly int MaxBits;

        public int MaxValue
        {
            get { return (1 << MaxBits) - 1; }
        }

        public ColorMap(int w, int h, int maxBits)
            : this(w, h, maxBits, new T[w * h * 3])
        {

        }

        public ColorMap(int w, int h, int maxBits, T[] rgb)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = rgb;
        }

        public ColorMap(ColorMap<T> m, T[] rgb)
        {
            Width = m.Width;
            Height = m.Height;
            MaxBits = m.MaxBits;
            Rgb = rgb;
        }

        public Color<T> GetPixel()
        {
            return GetPixel(0, 0);
        }

        public Color<T> GetPixel(int x, int y)
        {
            return new Color<T>(this, x, y);
        }

        public Color<T> GetRow(int y)
        {
            return new Color<T>(this, y);
        }

        public ColorMap<T> CopyAndUpdateColors(int newMaxBits, ColorMapProcessor processor)
        {
            var result = new ColorMap<T>(Width, Height, newMaxBits, new T[Width * Height * 3]);
            var input = GetPixel();
            var output = result.GetPixel();
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    processor(x, y, input, output);
                    input.MoveNext();
                    output.MoveNext();
                }
            return result;
        }

        public void ForEachPixel(Action<Color<T>> action)
        {
            var pix = GetPixel();
            do
            {
                action(pix);
            } while (pix.MoveNextAndCheck());
        }

        public void ForEachPixel(Action<int, T> action)
        {
            var pix = GetPixel();
            do
            {
                action(0, pix[0]);
                action(1, pix[1]);
                action(2, pix[2]);
            } while (pix.MoveNextAndCheck());
        }

        public static T[,] MakeCurve(int maxBits, Func<int, int, T> processor)
        {
            var newcurve = new T[3, 1 << maxBits];
            for (var c = 0; c < 3; c++)
                for (var i = 0; i < (1 << maxBits); i++)
                    newcurve[c, i] = processor(c, i);
            return newcurve;
        }
    }

    public static class ColorMapExtensions
    {
        public static Histogram GetHistogram(this ColorMap<ushort> map)
        {
            var result = new Histogram(map.MaxValue);

            map.ForEachPixel((comp, b) => result.AddValue(comp, b));
            return result;
        }
        public static Histogram GetHistogram(this RGB8Map map)
        {
            var result = new Histogram(255);

            map.ForEachPixel((comp, b) => result.AddValue(comp, b));
            return result;
        }
    }
}