using System;
using System.Collections;
using System.Collections.Generic;

namespace com.azi.image
{
    public class ColorMap<T> where T : IComparable<T>
    {
        public delegate void ColorMapProcessor(int x, int y, Color<T> input, Color<T> output);
        public delegate T CurveProcessor(int component, int index, T input);

        public readonly int Height;
        public readonly int MaxBits;
        public readonly T[] Rgb;
        public readonly int Width;
        public readonly T[,] Curve;

        public int MaxValue { get { return (1 << MaxBits) - 1; } }

        public ColorMap(int w, int h, int maxBits)
            : this(w, h, maxBits, new T[w * h * 3], new T[3, 1 << maxBits])
        {

        }

        public ColorMap(int w, int h, int maxBits, T[] rgb, T[,] curve)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = rgb;
            Curve = curve;
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

        public ColorMap<T> UpdateCurve(CurveProcessor processor)
        {
            var newcurve = new T[3, 1 << MaxBits];
            for (var c = 0; c < 3; c++)
                for (var i = 0; i <= MaxValue; i++)
                    newcurve[c, i] = processor(c, i, Curve[c, i]);
            return new ColorMap<T>(Width, Height, MaxBits, Rgb, newcurve);
        }

        public ColorMap<T> UpdateColors(int newMaxBits, ColorMapProcessor processor)
        {
            var result = new ColorMap<T>(Width, Height, newMaxBits);
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

        internal void Enumerate(Action<Color<T>> action)
        {
            var pix = GetPixel();
            do
            {
                action(pix);
            } while (pix.MoveNext());
        }
    }
}