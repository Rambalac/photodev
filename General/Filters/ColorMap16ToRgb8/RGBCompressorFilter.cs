﻿namespace com.azi.Filters.ColorMap16ToRgb8
{
    public class RGBCompressorFilter : IndependentColorComponentToRGBFilter
    {
        public override byte ProcessColor(float input, int component)
        {
            return (byte)(input * 255);
        }
    }
}