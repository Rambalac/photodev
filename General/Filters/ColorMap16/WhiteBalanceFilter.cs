using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class WhiteBalanceFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public ushort[] WhiteColor { get; private set; }
        //public int Precision { get; set; }

        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            var maxVal = image.Pixels.MaxValue;
            var whiteColor = image.Exif.WhiteColor ?? new[] { (ushort)maxVal, (ushort)maxVal, (ushort)maxVal };
            var mult = whiteColor.Select(v => maxVal / (float)v).ToArray();

            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.CopyAndUpdateCurve(
                    (component, index, input) => (ushort)Math.Min(maxVal, input * mult[component]))
            };
        }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            double maxbright = 0;
            var whiteColor = new ushort[] { 1, 1, 1 };
            var maxVal = (ushort)image.Pixels.MaxValue;
            image.Pixels.Enumerate(color =>
            {
                var bright = color.Brightness();
                if (bright < maxbright || color.MaxComponent() == maxVal) return;

                maxbright = bright;
                whiteColor = color.GetCopy();
            });
            var maxComp = whiteColor.Max();
            WhiteColor = whiteColor.Select(v => (ushort)(v * maxVal / maxComp)).ToArray();

        }
    }
}
