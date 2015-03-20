﻿using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters
{
    public abstract class ColorToColorFilter<TA, TB> : IColorToColorFilter<TA, TB>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void ProcessColor(TA[] input, int inputOffset, TB[] output, int outputOffset);

        public void ProcessColor(TA[] input, int inputOffset, Color<TB> output)
        {
            ProcessColor(input, inputOffset, output.Map, output.Offset);
        }

        public void ProcessColor(TA[] input, Color<TB> output)
        {
            ProcessColor(input, 0, output.Map, output.Offset);
        }

        public void ProcessColor(Color<TA> input, Color<TB> output)
        {
            ProcessColor(input.Map, input.Offset, output.Map, output.Offset);
        }

        public void ProcessColor(TA[] input, TB[] output)
        {
            ProcessColor(input, 0, output, 0);
        }
    }
}