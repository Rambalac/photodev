using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.Image;
using System.Numerics;
using static com.azi.Image.Vector3Extensions;

namespace com.azi.Filters.VectorMapFilters
{
    public class LightFilter : IndependentComponentVectorToVectorFilter,
        IAutoAdjustableFilter<ColorMapFloat>
    {
        private Vector3 _contrast = new Vector3(1f, 1f, 1f);
        private Vector3 _inoutLen = new Vector3(1f, 1f, 1f);
        private Vector3 _maxIn = new Vector3(1f, 1f, 1f);
        private Vector3 _maxOut = new Vector3(1f, 1f, 1f);
        private Vector3 _minIn = new Vector3(0f, 0f, 0f);
        private Vector3 _minOut = new Vector3(0f, 0f, 0f);

        public Vector3 Contrast
        {
            get { return _contrast; }
            set { _contrast = value; }
        }

        public Vector3 MinIn
        {
            get { return _minIn; }
            set
            {
                _minIn = value;
                Recalculate();
            }
        }

        public Vector3 MaxIn
        {
            get { return _maxIn; }
            set
            {
                _maxIn = value;
                Recalculate();
            }
        }

        public Vector3 MinOut
        {
            get { return _minOut; }
            set
            {
                _minOut = value;
                Recalculate();
            }
        }

        public Vector3 MaxOut
        {
            get { return _maxOut; }
            set
            {
                _maxOut = value;
                Recalculate();
            }
        }

        public void AutoAdjust(ColorMapFloat map)
        {
            const int maxValue = 1023;
            var h = map.GetHistogram(maxValue);

            var wcenter = h.FindWeightCenter(Vector3.Zero, Vector3.One);
            var wcenterf = (wcenter - _minIn) / (_maxIn - _minIn);
            _contrast = Log(new Vector3(0.5f), wcenterf);
            _contrast = _contrast.Average();

            //            h.Transform((index, value, comp) => (int)(1023 * Math.Pow(index / 1023f, _contrast[comp])));

            Vector3 max;
            Vector3 min;
            h.FindMinMax(out min, out max, 0.005f);
            //min = Enumerable.Repeat(min.Average(), 3).ToArray();
            //max = Enumerable.Repeat(max.Average(), 3).ToArray();

            _maxIn = max;
            _minIn = min;
            Recalculate();
        }

        public void SetContrast(float value)
        {
            _contrast = new Vector3(value, value, value);
        }

        private void Recalculate()
        {
            _inoutLen = (_maxOut - _minOut) / (MaxIn - MinIn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector3 ProcessColor(Vector3 input)
        {
            var f = Vector3.Max(Vector3.Zero, ((input - _minIn) * _inoutLen + _minOut));
            return Pow(f, _contrast);
        }
    }
}