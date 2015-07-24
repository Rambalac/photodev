using System.Numerics;
using com.azi.Image;

namespace com.azi.Filters.VectorMapFilters
{
    public class SaturationFilter : VectorToVectorFilter
    {
        private float _saturation = 1;

        public float Saturation
        {
            set { _saturation = value; }
            get { return _saturation; }
        }

        public override void ProcessColor(ref Vector3 input, ref Vector3 output)
        {
            var chroma = input.Average();
            output = chroma + (input - chroma) * _saturation;
        }
    }
}