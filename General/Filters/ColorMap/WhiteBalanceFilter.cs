using System;
using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class WhiteBalanceFilter : IColorMapFilter, IAutoAdjustableFilter
    {
        public int[] WhiteColor { get; private set; }
        //public int Precision { get; set; }

        public ColorImageFile Process(ColorImageFile image)
        {
            var maxVal = image.Pixels.MaxValue;
            if (WhiteColor == null && image.Exif.WhiteMultiplier != null)
            {
                WhiteColor = image.Exif.WhiteMultiplier.Select(v => (int)(maxVal * v)).ToArray();
            }
            if (WhiteColor == null) throw new Exception("WhiteColor is not defined neither in filter nor in Exif");

            return new ColorImageFile
            {
                Exif = image.Exif,
                Pixels = image.Pixels.UpdateCurve(
                    (component, index, input) => input * maxVal / WhiteColor[component])


            };
        }

        public void AutoAdjust(ColorImageFile image)
        {
            double maxbright = 0;
            var whiteColor = new[] { 1, 1, 1 };
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
