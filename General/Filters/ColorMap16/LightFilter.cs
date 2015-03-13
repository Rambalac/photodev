using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IndependentColorComponentFilter, IAutoAdjustableFilter
    {
        private float[] _inoutLen;
        private float[] _minIn = { 0f, 0f, 0f };
        private float[] _maxIn = { 1f, 1f, 1f };
        private float[] _minOut = { 0f, 0f, 0f };
        private float[] _maxOut = { 1f, 1f, 1f };

        public float[] MinIn
        {
            get { return _minIn; }
            set { _minIn = value; Recalculate(); }
        }

        public float[] MaxIn
        {
            get { return _maxIn; }
            set { _maxIn = value; Recalculate(); }
        }

        public float[] MinOut
        {
            get { return _minOut; }
            set { _minOut = value; Recalculate(); }
        }

        public float[] MaxOut
        {
            get { return _maxOut; }
            set { _maxOut = value; Recalculate(); }
        }

        private void Recalculate()
        {
            _inoutLen = MaxIn.Select((v, c) => (_maxOut[c] - _minOut[c]) / (v - MinIn[c])).ToArray();
        }

        public void AutoAdjust(ColorMap<ushort> map)
        {
            var histogram = map.GetHistogram();

            MaxIn = histogram.FindLastValueIndex(0.01).ToFloat(map.MaxBits);
            MinIn = histogram.FindFirstValueIndex(0.01).ToFloat(map.MaxBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ProcessColor(float input, int component)
        {
            return (input - _minIn[component]) * _inoutLen[component] + _minOut[component];
        }
    }
}