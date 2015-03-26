using System;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    public class WhiteBalanceFilter : IndependentComponentColorToColorFilter<float, float>,
        IAutoAdjustableFilter<ColorMapFloat>
    {
        private float[] _whiteColor = {1f, 1f, 1f};
        private float[] _whiteColor1 = {1f, 1f, 1f};

        public float[] WhiteColor
        {
            get { return _whiteColor; }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Should be array of 3");
                _whiteColor = value;
                _whiteColor1 = _whiteColor.Select(v => 1/v).ToArray();
            }
        }


        public void AutoAdjust(ColorMapFloat map)
        {
            double maxbright = 0;
            var whiteColor = new float[] {1, 1, 1};
            map.ForEachPixel(color =>
            {
                var bright = color.Brightness();
                if (bright < maxbright || color.MaxComponent() >= 1f) return;

                maxbright = bright;
                whiteColor = color.GetCopy();
            });
            var maxComp = whiteColor.Max();
            WhiteColor = whiteColor.Select(v => v/maxComp).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ProcessColor(float input, int component)
        {
            return input*_whiteColor1[component];
        }
    }
}