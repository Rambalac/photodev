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
            if (WhiteColor == null && image.Exif.WhiteMultiplier != null)
            {
                WhiteColor = image.Exif.WhiteMultiplier.Select(v => (ushort)(maxVal * v)).ToArray();
            }
            if (WhiteColor == null) throw new Exception("WhiteColor is not defined neither in filter nor in Exif");

            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.UpdateCurve(
                    (component, index, input) => (ushort)(input * maxVal / WhiteColor[component]))


            };
        }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            double maxbright = 0;
            var whiteColor = new ushort[] { 1, 1, 1 };
            image.Pixels.Enumerate(color =>
            {
                var bright = color.Brightness();
                if (!(bright > maxbright)) return;

                maxbright = bright;
                whiteColor = color.GetCopy();
            });

            WhiteColor = whiteColor.Normalize(image.Pixels.MaxBits);

        }
    }
}
