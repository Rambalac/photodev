using System.Runtime.CompilerServices;
using com.azi.Image;
using System.Numerics;

namespace com.azi.Filters
{
    public abstract class VectorToVectorFilter : IFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(ref Vector3 input, ref Vector3 output);

        public void ProcessColor(Vector3[] input, int inputOffset, Vector3[] output, int outputOffset)
        {
            ProcessColor(ref input[inputOffset], ref output[outputOffset]);
        }

        public void ProcessColor(Vector3[] input, int inputOffset, VectorPixel output)
        {
            ProcessColor(input, inputOffset, output.Map, output.Offset);
        }

        public void ProcessColor(Vector3 input, VectorPixel output)
        {
            ProcessColor(ref input, ref output.Map[output.Offset]);
        }

        public void ProcessColor(VectorPixel input, VectorPixel output)
        {
            ProcessColor(input.Map, input.Offset, output.Map, output.Offset);
        }
    }

    public abstract class VectorToColorFilter<T> : IFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(ref Vector3 input, ref T outR, ref T outG, ref T outB);

        public void ProcessColor(Vector3[] input, int inputOffset, T[] output, int outputOffset)
        {
            ProcessColor(ref input[inputOffset], ref output[outputOffset + 0], ref output[outputOffset + 1], ref output[outputOffset + 2]);
        }

        public void ProcessColor(Vector3[] input, int inputOffset, Color<T> output)
        {
            ProcessColor(input, inputOffset, output.Map, output.Offset);
        }

        public void ProcessColor(Vector3 input, Color<T> output)
        {
            ProcessColor(ref input, ref output.Map[output.Offset + 0], ref output.Map[output.Offset + 1], ref output.Map[output.Offset + 2]);
        }

        public void ProcessColor(VectorPixel input, Color<T> output)
        {
            ProcessColor(input.Map, input.Offset, output.Map, output.Offset);
        }
    }

}