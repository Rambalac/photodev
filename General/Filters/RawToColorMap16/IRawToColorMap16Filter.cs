using System;
using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16
{
    public interface IRawToColorMap16Filter<in T, TC> : IFilter
        where T : RawMap<TC>
    {
        ColorMap<TC> Process(T raw);
    }
}