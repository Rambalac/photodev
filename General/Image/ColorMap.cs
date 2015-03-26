using System;

namespace com.azi.Image
{
    public class ColorMap<T> : IColorMap
    {
        public delegate void ColorMapProcessor(int x, int y, Color<T> input, Color<T> output);

        public delegate T CurveProcessor(int component, int index, T input);

        public readonly T[] Rgb;
        private readonly int _height;
        private readonly int _width;

        public ColorMap(int w, int h)
            : this(w, h, new T[w*h*3])
        {
        }

        public ColorMap(int w, int h, T[] rgb)
        {
            _width = w;
            _height = h;
            Rgb = rgb;
        }

        public ColorMap(IColorMap m, T[] rgb)
        {
            _width = m.Width;
            _height = m.Height;
            Rgb = rgb;
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
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
            var result = new ColorMap<T>(_width, _height, new T[_width*_height*3]);
            var input = GetPixel();
            var output = result.GetPixel();
            for (var y = 0; y < _height; y++)
                for (var x = 0; x < _width; x++)
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
    }

    public class ColorMapFloat : ColorMap<float>
    {
        public ColorMapFloat(int w, int h)
            : this(w, h, new float[w*h*3])
        {
        }

        public ColorMapFloat(int w, int h, float[] rgb)
            : base(w, h, rgb)
        {
        }

        public ColorMapFloat(IColorMap m, float[] rgb)
            : this(m.Width, m.Height, rgb)
        {
        }

        public Histogram GetHistogram(int maxIndex)
        {
            var result = new Histogram(maxIndex);

            ForEachPixel((comp, b) => result.AddValue(comp, (int) (maxIndex*b)));
            return result;
        }
    }

    public class ColorMapUshort : ColorMap<ushort>
    {
        public readonly int MaxBits;

        public ColorMapUshort(int w, int h, int maxBits)
            : this(w, h, maxBits, new ushort[w*h*3])
        {
        }

        public ColorMapUshort(int w, int h, int maxBits, ushort[] rgb)
            : base(w, h, rgb)
        {
            MaxBits = maxBits;
        }

        public ColorMapUshort(ColorMapUshort m, ushort[] rgb)
            : this(m.Width, m.Height, m.MaxBits, rgb)
        {
        }

        public int MaxValue
        {
            get { return (1 << MaxBits) - 1; }
        }

        public Histogram GetHistogram()
        {
            var result = new Histogram(MaxValue);

            ForEachPixel((comp, b) => result.AddValue(comp, b));
            return result;
        }
    }
}