using System;
using System.Numerics;

namespace com.azi.Filters.ColorMap16ToRgb8
{
    public class ColorCompressorFilter : IndependentComponentColorToColorFilter<float, byte>
    {
        public override byte ProcessColor(float input, int component)
        {
            return (byte)(Math.Max(0, Math.Min(255, input * 255)));
        }
    }
    public class VectorCompressorFilter : IndependentComponentVectorToColorFilter<byte>
    {
        public override void ProcessColor(ref Vector3 input, ref byte outR, ref byte outG, ref byte outB)
        {
            var v = Vector3.Clamp(input, Vector3.Zero, Vector3.One);
            outR = (byte)(v.X * 255);
            outG = (byte)(v.Y * 255);
            outB = (byte)(v.Z * 255);
        }
    }
}