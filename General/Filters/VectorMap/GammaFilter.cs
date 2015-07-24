using System.Numerics;
using static com.azi.Image.Vector3Extensions;

namespace com.azi.Filters.VectorMapFilters
{
    public class GammaFilter : IndependentComponentVectorToVectorFilter
    {
        private Vector3 _gamma = new Vector3(2.2f, 2.2f, 2.2f);
        private Vector3 _gamma1 = new Vector3(1 / 2.2f, 1 / 2.2f, 1 / 2.2f);

        public GammaFilter()
        {
        }

        public GammaFilter(float gamma)
        {
            Gamma = new Vector3(gamma, gamma, gamma);
        }

        public Vector3 Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                _gamma1 = Vector3.Divide(Vector3.One, _gamma);
            }
        }

        public override Vector3 ProcessColor(Vector3 input) => Pow(input, _gamma1);
    }
}