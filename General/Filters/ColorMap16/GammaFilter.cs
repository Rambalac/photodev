using System;

namespace com.azi.Filters.ColorMap16
{
    public class GammaFilter : IndependentColorComponentFilter
    {
        public float[] Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        private float[] _gamma = { 2.2f, 2.2f, 2.2f };

        public override float ProcessColor(float input, int component)
        {
            return (float)Math.Pow(input, 1 / _gamma[component]);
        }
    }
}
