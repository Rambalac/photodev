using System;

namespace com.azi.Image
{
    public class RGB8Map : IColorMap
    {
        public const int BytesPerPixel = 3;

        readonly int _height;
        public readonly byte[] Rgb;
        public readonly int Stride;
        readonly int _width;

        public RGB8Map(int w, int h)
            : this(w, h, (4 * (w * 3 + 3) / 4), null)
        {
            Rgb = new byte[Stride * h * 3];
        }

        private RGB8Map(int width, int height, int stride, byte[] rgb)
        {
            this._width = width;
            this._height = height;
            Stride = stride;
            Rgb = rgb;
        }

        public Color<byte> GetRow(int y)
        {
            return new Color<byte>(Rgb, y * Stride, y * Stride + _width * 3);
        }

        public void ForEachPixel(Action<Color<byte>> action)
        {
            for (var y = 0; y < _height; y++)
            {
                var pix = GetRow(y);

                do
                {
                    action(pix);
                } while (pix.MoveNextAndCheck());
            }
        }

        public void ForEachPixel(Action<int, byte> action)
        {
            for (var y = 0; y < _height; y++)
            {
                var pix = GetRow(y);

                do
                {
                    action(0, pix[0]);
                    action(1, pix[1]);
                    action(2, pix[2]);
                } while (pix.MoveNextAndCheck());
            }
        }

        public Histogram GetHistogram()
        {
            var result = new Histogram(255);

            ForEachPixel((comp, b) => result.AddValue(comp, b));
            return result;
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
    }
}