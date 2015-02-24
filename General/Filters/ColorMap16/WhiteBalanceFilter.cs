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
            var maxVal = (1 << image.Pixels.MaxBits) - 1;
            if (WhiteColor == null && image.Exif.WhiteMultiplier != null)
            {
                WhiteColor = image.Exif.WhiteMultiplier.Select(v => (ushort)(maxVal * v)).ToArray();
            }
            if (WhiteColor == null) throw new Exception("WhiteColor is not defined neither in filter nor in Exif");

            return new ColorImageFile<ushort>
            {
                Exif = image.Exif,
                Pixels = image.Pixels.UpdateColors(image.Pixels.MaxBits,
                    delegate(int x, int y, Color<ushort> input, Color<ushort> output)
                    {
                        for (var c = 0; c < 3; c++)
                            output[c] = (ushort)(input[c] * maxVal / WhiteColor[c]);
                    })
            };
        }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            double maxbright = 0;
            var whiteColor = new ushort[] { 1, 1, 1 };
            foreach (var color in image.Pixels)
            {
                var bright = color.Brightness();
                if (!(bright > maxbright)) continue;

                maxbright = bright;
                whiteColor = color.Get();
            }

            WhiteColor = whiteColor.Normalize(image.Pixels.MaxBits);

        }
    }
}
