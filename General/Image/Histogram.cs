using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.azi.Image
{
    public class Histogram
    {
        public readonly int[] MaxValues;
        public readonly int[] MinIndex;
        public readonly int[] MaxIndex;
        public readonly int[][] Values;
        private int _maxIndex;

        public Histogram(int maxIndex)
        {
            _maxIndex = maxIndex;
            MaxValues = new int[3];
            Values = new[] { new int[maxIndex + 1], new int[maxIndex + 1], new int[maxIndex + 1] };
            MinIndex = new[] { maxIndex, maxIndex, maxIndex };
            MaxIndex = new[] { 0, 0, 0 };
        }

        public void AddValue(int comp, ushort index)
        {
            MinIndex[comp] = Math.Min(MinIndex[comp], index);
            MaxIndex[comp] = Math.Max(MaxIndex[comp], index);
            var val = Values[comp][index]++;
            MaxValues[comp] = Math.Max(MaxValues[comp], val);
        }

        public ushort[] FindFirstValueIndex(double e)
        {
            var result = new ushort[] { 0, 0, 0 };
            for (var c = 0; c < 3; c++)
            {
                var vals = Values[c];
                for (var i = MinIndex[c]; i <= MaxIndex[c]; i++)
                    if ((vals[i] / (double)MaxValues[c]) > e)
                    {
                        result[c] = (ushort)i;
                        break;
                    }
            }

            return result;
        }
        public ushort[] FindLastValueIndex(double e)
        {
            var result = new ushort[] { (ushort)_maxIndex, (ushort)_maxIndex, (ushort)_maxIndex };
            for (var c = 0; c < 3; c++)
            {
                var vals = Values[c];
                for (var i = MaxIndex[c]; i >= MinIndex[c]; i--)
                    if ((vals[i] / (double)MaxValues[c]) > e)
                    {
                        result[c] = (ushort)i;
                        break;
                    }
            }

            return result;
        }
    }
}
