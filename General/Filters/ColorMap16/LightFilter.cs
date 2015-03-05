using System;
using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        private const double DefaultGamma = 2.2;
        public double? Brightness { get; set; }
        public double? Gamma { get; set; }
        public double? Contrast { get; set; }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var max = 1;
            image.Pixels.Enumerate(color => max = Math.Max(max, color.MaxComponent()));
        }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            var maxValue = image.Pixels.MaxValue;
            var brightness = Brightness ?? 0;
            var gamma = Gamma ?? DefaultGamma;
            var contrast = Contrast ?? 1;
            var powtable = new ushort[maxValue + 1];
            for (var i = 0; i <= maxValue; i++)
                powtable[i] = (ushort)Math.Max(0, Math.Min(maxValue, maxValue * (brightness + contrast * Math.Pow((i / (double)maxValue), 1 / gamma))));
            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.CopyAndUpdateCurve((component, index, input) => powtable[input])
            };
        }
    }
}