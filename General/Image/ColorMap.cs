using System;

namespace com.azi.image
{
    public class ColorMap
    {
        public delegate void ColorMapProcessor(int x, int y, Color input, Color output);
        public delegate int CurveProcessor(int component, int index, int input);

        public readonly int Height;
        public readonly int MaxBits;
        public readonly int[] Rgb;
        public readonly int Width;
        public readonly int[,] Curve;

        public int MaxValue { get { return (1 << MaxBits) - 1; } }

        public ColorMap(int w, int h, int maxBits)
            : this(w, h, maxBits, new int[w * h * 3], new int[3, 1 << maxBits])
        {

        }

        public ColorMap(int w, int h, int maxBits, int[] rgb, int[,] curve)
        {
            Width = w;
            Height = h;
            MaxBits = maxBits;
            Rgb = rgb;
            Curve = curve;
        }

        public Color GetPixel()
        {
            return GetPixel(0, 0);
        }

        public Color GetPixel(int x, int y)
        {
            return new Color(this, x, y);
        }

        public Color GetRow(int y)
        {
            return new Color(this, y);
        }

        public ColorMap UpdateCurve(CurveProcessor processor)
        {
            var newcurve = new int[3, 1 << MaxBits];
            for (var c = 0; c < 3; c++)
                for (var i = 0; i <= MaxValue; i++)
                    newcurve[c, i] = processor(c, i, Curve[c, i]);
            return new ColorMap(Width, Height, MaxBits, Rgb, newcurve);
        }

        public ColorMap UpdateColors(int newMaxBits, ColorMapProcessor processor)
        {
            var result = new ColorMap(Width, Height, newMaxBits);
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

        internal void Enumerate(Action<Color> action)
        {
            var pix = GetPixel();
            do
            {
                action(pix);
            } while (pix.MoveNext());
        }
    }
}