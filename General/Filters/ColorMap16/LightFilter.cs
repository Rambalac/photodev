using System;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IndependentComponentColorToColorFilter<float, float>, IAutoAdjustableFilter<ColorMapFloat>
    {
        private float[] _inoutLen = { 1f, 1f, 1f };
        private float[] _minIn = { 0f, 0f, 0f };
        private float[] _maxIn = { 1f, 1f, 1f };
        private float[] _minOut = { 0f, 0f, 0f };
        private float[] _maxOut = { 1f, 1f, 1f };
        private float[] _contrast = { 1f, 1f, 1f };

        public float[] Contrast
        {
            get { return _contrast; }
            set { _contrast = value; }
        }

        public float[] MinIn
        {
            get { return _minIn; }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Should be array of 3");
                _minIn = value; Recalculate();
            }
        }

        public float[] MaxIn
        {
            get { return _maxIn; }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Should be array of 3");
                _maxIn = value; Recalculate();
            }
        }

        public float[] MinOut
        {
            get { return _minOut; }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Should be array of 3");
                _minOut = value; Recalculate();
            }
        }

        public float[] MaxOut
        {
            get { return _maxOut; }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Should be array of 3");
                _maxOut = value; Recalculate();
            }
        }

        private void Recalculate()
        {
            _inoutLen = MaxIn.Select((v, c) => (_maxOut[c] - _minOut[c]) / (v - MinIn[c])).ToArray();
        }

        public void AutoAdjust(ColorMapFloat map)
        {
            const int maxValue = 1023;
            var h = map.GetHistogram(maxValue);

            var wcenter = h.FindWeightCenter();
            var wcenterf = wcenter.Select((v, c) => (v - _minIn[c]) / (_maxIn[c] - _minIn[c]));
            //_contrast = wcenterf.Select(v => (float)Math.Log(0.3, v)).ToArray();
            //_contrast = Enumerable.Repeat(wcenterf.Select(v => (float)Math.Log(0.4, v)).Average(), 3).ToArray();

            h.Transform((index, value, comp) => (int)(1023 * Math.Pow(index / 1023f, _contrast[comp])));

            float[] max;
            float[] min;
            h.FindMinMax(out min, out max, 0.005f);

            _maxIn = max;
            _minIn = min;
            Recalculate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ProcessColor(float input, int component)
        {
            return (float)Math.Pow(((input - _minIn[component]) * _inoutLen[component] + _minOut[component]), _contrast[component]);
        }
    }
}