using System;

namespace com.azi.Filters.ColorMap16ToRgb8
{
    public class ColorCompressorFilter : IndependentComponentColorToColorFilter<float, byte>
    {
        public override byte ProcessColor(float input, int component)
        {
            return (byte) (Math.Max(0, Math.Min(255, input*255)));
        }
    }
}