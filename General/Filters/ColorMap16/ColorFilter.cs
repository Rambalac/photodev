using System.Runtime.CompilerServices;

namespace com.azi.Filters.ColorMap16
{
    public interface IColorFilter : IFilter
    {
        void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset);
    }

    public abstract class ColorFilter : IColorFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset);

        public void ProcessColor(float[] input, float[] output)
        {
            ProcessColor(input, 0, output, 0);
        }
    }

    public abstract class IndependentColorComponentFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract float ProcessColor(float input, int component);

        public void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset)
        {
            output[outputOffset + 0] = ProcessColor(input[inputOffset + 0], 0);
            output[outputOffset + 1] = ProcessColor(input[inputOffset + 1], 1);
            output[outputOffset + 2] = ProcessColor(input[inputOffset + 1], 2);
        }
    }
}
