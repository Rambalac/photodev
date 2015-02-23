using System;
using System.Collections;
using System.Collections.Generic;

namespace com.azi.image
{
    public class ColorMap<T> : IEnumerable<Color<T>> where T : IComparable<T>
    {
        public delegate void ColorMapProcessor(int x, int y, Color<T> input, Color<T> output);

        public readonly int Height;
        public readonly int MaxBits;
        public readonly T[] Rgb;
        public readonly int Width;

        public ColorMap(int w, int h, int maxBits)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = new T[w * h * 3];
        }

        public IEnumerator<Color<T>> GetEnumerator()
        {
            return GetPixel();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
    }
}