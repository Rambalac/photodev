using System;
using System.Linq;
using System.Reflection;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        private readonly double[] _defaultGamma = { 2.2, 2.2, 2.2 };
        public double[] Gamma { get; set; }
        public double[] Contrast { get; set; }
        public double[] MinIn { get; set; }
        public double[] MaxIn { get; set; }
        public double[] MinOut { get; set; }
        public double[] MaxOut { get; set; }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var maxValue = image.Pixels.MaxValue;
            var min = new[] { (ushort)maxValue, (ushort)maxValue, (ushort)maxValue };
            var max = new ushort[] { 1, 1, 1 };
            image.Pixels.Enumerate((component, value) =>
            {
                max[component] = Math.Max(max[component], image.Pixels.Curve[component, value]);
                min[component] = Math.Min(min[component], image.Pixels.Curve[component, value]);
            });

            var gamma = Gamma ?? _defaultGamma;
            MaxIn = max.Select((v, c) => GammaFix(v, maxValue, gamma[c])).ToArray();
            MinIn = min.Select((v, c) => GammaFix(v, maxValue, gamma[c])).ToArray();

            //Contrast = max.Zip(gamma, (m, g) => 1 / GammaFix(m, maxValue, g)).ToArray();
        }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            var maxValue = image.Pixels.MaxValue;
            var gamma = Gamma ?? _defaultGamma;
            var contrast = Contrast ?? new[] { 1.0, 1.0, 1.0 };
            var minIn = MinIn ?? new[] { 0.0, 0.0, 0.0 };
            var maxIn = MaxIn ?? new[] { 1.0, 1.0, 1.0 };
            var minOut = MinOut ?? new[] { 0.0, 0.0, 0.0 };
            var maxOut = MaxOut ?? new[] { 1.0, 1.0, 1.0 };

            var inLen = maxIn.Select((v, c) => v - minIn[c]).ToArray();
            var outLen = maxOut.Zip(minOut, (v1, v2) => v1 - v2).ToArray();

            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.CopyAndUpdateCurve((component, index, input) => (ushort)Math.Max(0, Math.Min(maxValue,
                    maxValue * (
                        Math.Pow(Math.Max(0, (GammaFix(input, maxValue, gamma[component]) - minIn[component])) / inLen[component], contrast[component]) * outLen[component] + minOut[component]
                    )
                )))
            };
        }

        private static double GammaFix(ushort input, int maxValue, double gamma)
        {
            return Math.Pow((input / (double)maxValue), 1 / gamma);
        }
    }
}