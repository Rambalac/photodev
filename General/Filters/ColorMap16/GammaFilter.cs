using System;
using System.Linq;

namespace com.azi.Filters.ColorMap16
{
    public class GammaFilter : IndependentComponentColorToColorFilter<float, float>
    {
        public float[] Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                _gamma1 = _gamma.Select(v => 1 / v).ToArray();
            }
        }

        private float[] _gamma = { 2.2f, 2.2f, 2.2f };
        private float[] _gamma1 = { 1 / 2.2f, 1 / 2.2f, 1 / 2.2f };

        public GammaFilter()
        {

        }

        public GammaFilter(float gamma)
        {
            Gamma = Enumerable.Repeat(gamma, 3).ToArray();
        }

        public override float ProcessColor(float input, int component)
        {
            return (float)Math.Pow(input, _gamma1[component]);
        }
    }
}
