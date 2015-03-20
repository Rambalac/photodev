using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.azi.Filters.ColorMap16
{
    public class SaturationFilter : ColorToColorFilter<float, float>
    {
        public float Saturation
        {
            set { _saturation = value; }
            get { return _saturation; }
        }
        private float _saturation = 1;

        public override void ProcessColor(float[] input, int inputOffset, float[] output, int outputOffset)
        {
            var r = input[inputOffset + 0];
            var g = input[inputOffset + 1];
            var b = input[inputOffset + 2];
            var chroma = (r + g + b) / 3;
            output[outputOffset + 0] = chroma + (r - chroma) * _saturation;
            output[outputOffset + 1] = chroma + (g - chroma) * _saturation;
            output[outputOffset + 2] = chroma + (b - chroma) * _saturation;
        }
    }
}
