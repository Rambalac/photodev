using System;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;
using System.Numerics;

namespace com.azi.Filters.VectorMapFilters
{
    public class WhiteBalanceFilter : IndependentComponentVectorToVectorFilter,
        IAutoAdjustableFilter<VectorMap>
    {
        private Vector3 _whiteColor = Vector3.One;
        private Vector3 _whiteColor1 = Vector3.One;

        public Vector3 WhiteColor
        {
            get { return _whiteColor; }
            set
            {
                _whiteColor = value;
                _whiteColor1 = Vector3.One / _whiteColor;
            }
        }


        public void AutoAdjust(com.azi.Image.VectorMap map)
        {
            double maxbright = 0;
            Vector3 whiteColor = Vector3.One;
            map.ForEachPixel(color =>
            {
                var bright = color.Value.LengthSquared();
                if (bright < maxbright || color.Value.MaxComponent() >= 1f) return;

                maxbright = bright;
                whiteColor = color.Value;
            });
            var maxComp = whiteColor.MaxComponent();
            WhiteColor = whiteColor / maxComp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector3 ProcessColor(Vector3 input)
        {
            return input * _whiteColor1;
        }
    }
}