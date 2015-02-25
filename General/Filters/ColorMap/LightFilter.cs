using System;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMapFilter, IAutoAdjustableFilter
    {
        public double Multiply { get; set; }
        public double Gamma { get; set; }

        public void AutoAdjust(ColorImageFile image)
        {
            var max = 0;
            image.Pixels.Enumerate(color => max = Math.Max(max, color.MaxComponent()));
            Multiply = image.Pixels.MaxValue / (double)max;
            if (Multiply < 1) Multiply = 1;
        }

        public ColorImageFile Process(ColorImageFile image)
        {
            var maxValue = image.Pixels.MaxValue;
            const int precision = 8;
            var multiply = (int)(Multiply * (1 << precision));
            var powtable = new int[maxValue + 1];
            for (var i = 0; i <= maxValue; i++)
                powtable[i] = (int)(maxValue * Math.Pow((i / (double)maxValue), Gamma));
            return new ColorImageFile
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