using System;
using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public double Multiply { get; set; }
        public double Gamma { get; set; }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var max = 0;
            image.Pixels.Enumerate(color => max = Math.Max(max, color.MaxComponent()));
            Multiply = image.Pixels.MaxValue / (double)max;
            if (Multiply < 1) Multiply = 1;
        }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            var maxValue = image.Pixels.MaxValue;
            const int precision = 8;
            var multiply = (int)(Multiply * (1 << precision));
            var powtable = new ushort[maxValue + 1];
            for (var i = 0; i <= maxValue; i++)
                powtable[i] = (ushort)(maxValue * Math.Pow((i / (double)maxValue), Gamma));
            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.UpdateCurve((component, index, input) =>
                {
                    var outval = (index * multiply) >> precision;
                    if (outval > maxValue) outval = maxValue;
                    return powtable[outval];
                })
            };
        }
    }
}