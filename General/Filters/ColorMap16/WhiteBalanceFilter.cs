﻿using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    public class WhiteBalanceFilter : IndependentColorComponentFilter, IAutoAdjustableFilter
    {
        public float[] WhiteColor
        {
            get { return _whiteColor; }
            private set { _whiteColor = value; }
        }

        private float[] _whiteColor = { 1f, 1f, 1f };


        public void AutoAdjust(ColorMap<ushort> map)
        {
            double maxbright = 0;
            var whiteColor = new ushort[] { 1, 1, 1 };
            var maxVal = (ushort)map.MaxValue;
            map.ForEachPixel(color =>
            {
                var bright = color.Brightness();
                if (bright < maxbright || color.MaxComponent() == maxVal) return;

                maxbright = bright;
                whiteColor = color.GetCopy();
            });
            var maxComp = (float)whiteColor.Max();
            WhiteColor = whiteColor.Select(v => v / maxComp).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ProcessColor(float input, int component)
        {
            return input / _whiteColor[component];
        }
    }
}
