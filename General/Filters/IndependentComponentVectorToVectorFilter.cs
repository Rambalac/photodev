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
            ProcessColor(input[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(ref Vector3 input, ref T[] output)
        {
            output = ProcessColor(input);

#if DEBUG
            if (typeof (TB) is float[])
            {
                if ((output as float[])[outputOffset + 0] > 1 || float.IsNaN((output as float[])[outputOffset + 0])) throw new Exception();
                if ((output as float[])[outputOffset + 1] > 1 || float.IsNaN((output as float[])[outputOffset + 1])) throw new Exception();
                if ((output as float[])[outputOffset + 2] > 1 || float.IsNaN((output as float[])[outputOffset + 2])) throw new Exception();
            }
#endif
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

        public override void ProcessColor(ref Vector3 input,ref Vector3 output)
        {
            output = ProcessColor(input);

#if DEBUG
            if (typeof (TB) is float[])
            {
                if ((output as float[])[outputOffset + 0] > 1 || float.IsNaN((output as float[])[outputOffset + 0])) throw new Exception();
                if ((output as float[])[outputOffset + 1] > 1 || float.IsNaN((output as float[])[outputOffset + 1])) throw new Exception();
                if ((output as float[])[outputOffset + 2] > 1 || float.IsNaN((output as float[])[outputOffset + 2])) throw new Exception();
            }
#endif
        }
    }
}