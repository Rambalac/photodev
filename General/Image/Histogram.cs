using System;

namespace com.azi.Image
{
    public class Histogram
    {
        public readonly int[] MaxValues;
        public readonly int[] MinIndex;
        public readonly int[] MaxIndex;
        public readonly int[][] Values;
        public int TotalPixels;
        private readonly int _maxIndex;

        public Histogram(int maxIndex)
        {
            _maxIndex = maxIndex;
            MaxValues = new int[3];
            Values = new[] { new int[maxIndex + 1], new int[maxIndex + 1], new int[maxIndex + 1] };
            MinIndex = new[] { maxIndex, maxIndex, maxIndex };
            MaxIndex = new[] { 0, 0, 0 };
        }

        public void AddValue(int comp, int index)
        {
            if (comp == 0) TotalPixels++;
            MinIndex[comp] = Math.Min(MinIndex[comp], index);
            MaxIndex[comp] = Math.Max(MaxIndex[comp], index);
            var val = Values[comp][index]++;
            MaxValues[comp] = Math.Max(MaxValues[comp], val);
        }

        public ushort[] FindWeightCenter(ushort[] min, ushort[] max)
        {
            var result = new ushort[3];
            for (var c = 0; c < 3; c++)
            {
                var vals = Values[c];
                var minsum = 0;
                var maxsum = 0;
                var mini = min[c];
                var maxi = max[c];

                do
                {
                    while (minsum <= maxsum && mini < maxi) minsum += vals[mini++];
                    while (maxsum < minsum && maxi > mini) maxsum += vals[maxi--];
                } while (mini < maxi);

                result[c] = mini;
            }
            return result;
        }

        public void FindMinMax(out ushort[] min, out ushort[] max)
        {
            max = new[] { (ushort)_maxIndex, (ushort)_maxIndex, (ushort)_maxIndex };
            min = new ushort[] { 0, 0, 0 };

            const float e = 0.005f;
            var amount = (int)(e * TotalPixels);
            for (var c = 0; c < 3; c++)
            {
                var vals = Values[c];
                var minsum = 0;
                var maxsum = 0;
                var start = Math.Min(1, Math.Min(MinIndex[c], _maxIndex - MaxIndex[c]));

                for (var i = start; i < _maxIndex; i++)
                {
                    minsum += vals[i];
                    if (minsum < amount)
                    {
                        min[c] = (ushort)i;
                    }

                    maxsum += vals[_maxIndex - i];
                    if (maxsum < amount)
                    {
                        max[c] = (ushort)(_maxIndex - i);
                    }

                }
            }

        }

    }
}
