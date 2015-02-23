using System.Linq;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public ushort Multiply { get; set; }
        public int Precision = 2;

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            var max = image.Pixels.Max(c => c.MaxComponent());
            Multiply = (ushort)((1 << (image.Pixels.MaxBits + Precision)) / max);
            if (Multiply == 0) Multiply = 1;
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
                            output[c] = (ushort)((input[c] * Multiply) >> Precision);
                    })
            };
        }
    }
}