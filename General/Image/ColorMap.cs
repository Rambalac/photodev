using System;
using System.Collections;
using System.Collections.Generic;
using com.azi.Image;

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
        public readonly float[,] ColorMatrix;

        public int MaxValue
        {
            get { return (1 << MaxBits) - 1; }
        }

        public ColorMap(int w, int h, int maxBits, float[,] colorMatrix)
            : this(w, h, maxBits, new T[w * h * 3], new T[3, 1 << maxBits], colorMatrix)
        {

        }

        public ColorMap(int w, int h, int maxBits, T[] rgb, T[,] curve, float[,] colorMatrix)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = rgb;
            Curve = curve;
            ColorMatrix = colorMatrix;
        }

        public ColorMap(ColorMap<T> m, T[,] curve)
        {
            Width = m.Width;
            Height = m.Height;
            MaxBits = m.MaxBits;
            Rgb = m.Rgb;
            Curve = curve;
            ColorMatrix = m.ColorMatrix;
        }

        public ColorMap(ColorMap<T> m, T[] rgb)
        {
            Width = m.Width;
            Height = m.Height;
            MaxBits = m.MaxBits;
            Rgb = rgb;
            Curve = m.Curve;
            ColorMatrix = m.ColorMatrix;
        }

        public ColorMap(ColorMap<T> m, float[,] colorMatrix)
        {
            Width = m.Width;
            Height = m.Height;
            MaxBits = m.MaxBits;
            Rgb = m.Rgb;
            Curve = m.Curve;
            ColorMatrix = colorMatrix;
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

        public ColorMap<T> CopyAndUpdateCurve(CurveProcessor processor)
        {
            var newcurve = new T[3, 1 << MaxBits];
            for (var c = 0; c < 3; c++)
                for (var i = 0; i <= MaxValue; i++)
                    newcurve[c, i] = processor(c, i, Curve[c, i]);
            return new ColorMap<T>(this, newcurve);
        }

        public void UpdateCurve(CurveProcessor processor)
        {
            var newcurve = new T[3, 1 << MaxBits];
            for (var c = 0; c < 3; c++)
                for (var i = 0; i <= MaxValue; i++)
                    Curve[c, i] = processor(c, i, Curve[c, i]);
        }

        public ColorMap<T> CopyAndUpdateColors(int newMaxBits, ColorMapProcessor processor)
        {
            var result = new ColorMap<T>(Width, Height, newMaxBits, new T[Width * Height * 3], Curve, ColorMatrix);
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

        public void Enumerate(Action<Color<T>> action)
        {
            var pix = GetPixel();
            do
            {
                action(pix);
            } while (pix.MoveNext());
        }

        public void Enumerate(Action<int, T> action)
        {
            var pix = GetPixel();
            do
            {
                action(0, pix[0]);
                action(1, pix[1]);
                action(2, pix[2]);
            } while (pix.MoveNext());
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

            map.Enumerate((comp, value) => result.AddValue(comp, map.Curve[comp, value]));
            return result;
        }
    }
}