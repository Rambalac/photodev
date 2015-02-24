using System;
using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public ushort Multiply { get; set; }
        public int Precision = 3;
        public ushort[,] Curve;

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var max = image.Pixels.Max(c => c.MaxComponent());
            Multiply = (ushort)((1 << (image.Pixels.MaxBits + Precision)) / max);
            if (Multiply == 0) Multiply = 1;

            var maxVal = 1 << image.Pixels.MaxBits;
            Curve = new ushort[3, maxVal];
            for (var c = 0; c < 3; c++)
                for (var v = 0; v < maxVal; v++)
                    Curve[c, v] = (ushort)v;
        }

        public void SetGamma(double gamma, ColorImageFile<ushort> image)
        {
            var maxVal = 1 << image.Pixels.MaxBits;
            Curve = new ushort[3, maxVal];
            for (var c = 0; c < 3; c++)
                for (var v = 0; v < maxVal; v++)
                    Curve[c, v] = (ushort)(maxVal * (Math.Pow((v / (double)maxVal), gamma)));
        }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.UpdateColors(image.Pixels.MaxBits,
                    delegate(int x, int y, Color<ushort> input, Color<ushort> output)
                    {
                        for (var c = 0; c < 3; c++)
                        {
                            var outval = (ushort)((input[c] * Multiply) >> Precision);
                            if (Curve != null) outval = Curve[c, outval];
                            output[c] = outval;
                        }
                    })
            };
        }
    }
}