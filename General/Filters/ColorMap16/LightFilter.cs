using System;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    public class LightFilter : IndependentColorComponentFilter, IAutoAdjustableFilter
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

        public void AutoAdjust(ColorMap<ushort> map)
        {
            var h = map.GetHistogram();

            ushort[] max;
            ushort[] min;
            h.FindMinMax(out min, out max);
            var wcenter = h.FindWeightCenter(min, max);

            _maxIn = max.ToFloat(map.MaxBits);
            _minIn = min.ToFloat(map.MaxBits);
            var wcenterf = wcenter.ToFloat(map.MaxBits).Select((v, c) => (v - _minIn[c]) / (_maxIn[c] - _minIn[c]));
            _contrast = wcenterf.Select(v => (float)Math.Log(0.5, v)).ToArray();
            Recalculate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ProcessColor(float input, int component)
        {
            return (float)Math.Pow(((input - _minIn[component]) * _inoutLen[component] + _minOut[component]), _contrast[component]);
        }
    }
}