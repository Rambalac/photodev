using System;
using System.Runtime.CompilerServices;

namespace com.azi.Filters
{
    public abstract class IndependentComponentColorToColorFilter<TA, TB> : ColorToColorFilter<TA, TB>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TB ProcessColor(TA input, int component);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProcessColorInCurve(int index, TA[][] input, TB[][] output)
        {
            output[0][index] = ProcessColor(input[0][index], 0);
            output[1][index] = ProcessColor(input[1][index], 1);
            output[2][index] = ProcessColor(input[2][index], 2);
        }

        public override void ProcessColor(TA[] input, int inputOffset, TB[] output, int outputOffset)
        {
            output[outputOffset + 0] = ProcessColor(input[inputOffset + 0], 0);
            output[outputOffset + 1] = ProcessColor(input[inputOffset + 1], 1);
            output[outputOffset + 2] = ProcessColor(input[inputOffset + 2], 2);

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