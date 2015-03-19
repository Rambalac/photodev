using System;
using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16.Demosaic
{
    public interface IDebayer<in T, TC> : IRawToColorMap16Filter<T, TC>
        where T : RawMap<TC>
    {
    }

    public interface IBGGRDebayer<T> : IDebayer<RawBGGRMap<T>, T>
    {
    }
}