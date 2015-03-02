using System;
using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public double? Multiplier { get; set; }
        public double? Gamma { get; set; }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var max = 1;
            image.Pixels.Enumerate(color => max = Math.Max(max, color.MaxComponent()));
            Multiplier = image.Pixels.MaxValue / (double)max;
            if (Multiplier < 1) Multiplier = 1;
        }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            var maxValue = image.Pixels.MaxValue;
            var multiply = Multiplier ?? image.Exif.Multiplier ?? 1;
            var gamma = Gamma ?? 1.0;
            var powtable = new ushort[maxValue + 1];
            if (Math.Abs(gamma - 1) > 0.0001)
            {
                for (var i = 0; i <= maxValue; i++)
                    powtable[i] = (ushort)Math.Min(maxValue, maxValue * multiply * Math.Pow((i / (double)maxValue), gamma));
            }
            else
            {
                for (var i = 0; i <= maxValue; i++)
                    powtable[i] = (ushort)Math.Min(maxValue, i * multiply);
            }
            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.CopyAndUpdateCurve((component, index, input) => powtable[input])
            };
        }
    }
}