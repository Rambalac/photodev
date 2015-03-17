using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16ToRgb8
{
    public interface IColorToRGBFilter : IFilter
    {
        void ProcessColor(float[] input, int inputOffset, byte[] output, int outputOffset);
    }

    public abstract class ColorToRgbFilter : IColorToRGBFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(float[] input, int inputOffset, byte[] output, int outputOffset);

        public void ProcessColor(float[] input, int inputOffset, Color<byte> output)
        {
            ProcessColor(input, inputOffset, output.Map, output.Offset);
        }

        public void ProcessColor(float[] input, Color<byte> output)
        {
            ProcessColor(input, 0, output.Map, output.Offset);
        }

        public void ProcessColor(float[] input, byte[] output)
        {
            ProcessColor(input, 0, output, 0);
        }
    }

    public abstract class IndependentColorComponentToRGBFilter : ColorToRgbFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract byte ProcessColor(float input, int component);

        public override void ProcessColor(float[] input, int inputOffset, byte[] output, int outputOffset)
        {
            output[outputOffset + 0] = ProcessColor(input[inputOffset + 0], 0);
            output[outputOffset + 1] = ProcessColor(input[inputOffset + 1], 1);
            output[outputOffset + 2] = ProcessColor(input[inputOffset + 2], 2);
        }
    }

}