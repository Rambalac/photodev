using System;
using System.Collections.Generic;
using System.Linq;

namespace com.azi.Image
{
    public class Histogram
    {
        public delegate int HistogramTransformFunc(int index, int value, int comp);

        public readonly int[] MaxIndex;

        public readonly int[] MaxValues;
        public readonly int[] MinIndex;
        public readonly int[][] Values;
        private readonly int _maxIndex;
        public int TotalPixels;

        public Histogram(int maxIndex)
        {
            _maxIndex = maxIndex;
            MaxValues = new int[3];
            Values = new[] {new int[maxIndex + 1], new int[maxIndex + 1], new int[maxIndex + 1]};
            MinIndex = new[] {maxIndex, maxIndex, maxIndex};
            MaxIndex = new[] {0, 0, 0};
        }

        public void AddValue(int comp, int index)
        {
            if (comp == 0) TotalPixels++;
            if (index > _maxIndex) index = _maxIndex;
            MinIndex[comp] = Math.Min(MinIndex[comp], index);
            MaxIndex[comp] = Math.Max(MaxIndex[comp], index);
            int val = Values[comp][index]++;
            MaxValues[comp] = Math.Max(MaxValues[comp], val);
        }

        public ushort[] FindWeightCenter(ushort[] min, ushort[] max)
        {
            int[] result = FindWeightCenter(min.Cast<int>().ToArray(), max.Cast<int>().ToArray());
            return result.Cast<ushort>().ToArray();
        }

        public float[] FindWeightCenter(float[] min = null, float[] max = null)
        {
            if (min == null) min = new[] {0f, 0f, 0f};
            if (max == null) max = new[] {1f, 1f, 1f};

            int[] result = FindWeightCenter(FromFloat(min), FromFloat(max));
            return ToFloat(result);
        }

        private int[] FromFloat(IEnumerable<float> a)
        {
            return a.Select(v => (int) (v*_maxIndex)).ToArray();
        }

        private float[] ToFloat(IEnumerable<int> a)
        {
            return a.Select(v => v/(float) _maxIndex).ToArray();
        }

        public void Transform(HistogramTransformFunc func)
        {
            var newval = new[] {new int[_maxIndex + 1], new int[_maxIndex + 1], new int[_maxIndex + 1]};
            for (int c = 0; c < 3; c++)
                for (int i = 0; i <= _maxIndex; i++)
                    newval[c][func(i, Values[c][i], c)] += Values[c][i];
        }

        public int[] FindWeightCenter(int[] min, int[] max)
        {
            var result = new int[3];
            for (int c = 0; c < 3; c++)
            {
                int[] vals = Values[c];
                int minsum = 0;
                int maxsum = 0;
                int mini = min[c];
                int maxi = max[c];

                do
                {
                    while (minsum <= maxsum && mini < maxi) minsum += vals[mini++];
                    while (maxsum < minsum && maxi > mini) maxsum += vals[maxi--];
                } while (mini < maxi);

                result[c] = mini;
            }
            return result;
        }

        public void FindMinMax(out ushort[] minout, out ushort[] maxout, float e = 0.005f)
        {
            int[] min, max;
            FindMinMax(out min, out max, e);
            minout = min.Cast<ushort>().ToArray();
            maxout = max.Cast<ushort>().ToArray();
        }

        public void FindMinMax(out float[] minout, out float[] maxout, float e = 0.005f)
        {
            int[] min, max;
            FindMinMax(out min, out max, e);
            minout = ToFloat(min);
            maxout = ToFloat(max);
        }

        public void FindMinMax(out int[] min, out int[] max, float e = 0.005f)
        {
            max = new[] {_maxIndex, _maxIndex, _maxIndex};
            min = new[] {0, 0, 0};

            var amount = (int) (e*TotalPixels);
            for (int c = 0; c < 3; c++)
            {
                int[] vals = Values[c];
                int minsum = 0;
                int maxsum = 0;
                int start = Math.Min(1, Math.Min(MinIndex[c], _maxIndex - MaxIndex[c]));

                for (int i = start; i < _maxIndex; i++)
                {
                    minsum += vals[i];
                    if (minsum < amount)
                    {
                        min[c] = (ushort) i;
                    }

                    maxsum += vals[_maxIndex - i];
                    if (maxsum < amount)
                    {
                        max[c] = (ushort) (_maxIndex - i);
                    }
                }
            }
        }
    }
}