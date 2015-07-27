using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace com.azi.Filters
{
    public abstract class IndependentComponentVectorToColorFilter<T> : VectorToColorFilter<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProcessColorInCurve(int index, Vector3[] input, T[][] output)
        {
            ProcessColor(ref input[index], ref output[0][index], ref output[1][index], ref output[2][index]);
        }
    }

    public abstract class IndependentComponentVectorToVectorFilter : VectorToVectorFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract Vector3 ProcessColor(Vector3 input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProcessColorInCurve(int index, Vector3[] input, Vector3[] output)
        {
            output[index] = ProcessColor(input[index]);
        }

        public override void ProcessColor(ref Vector3 input, ref Vector3 output)
        {
            output = ProcessColor(input);
        }
    }
}