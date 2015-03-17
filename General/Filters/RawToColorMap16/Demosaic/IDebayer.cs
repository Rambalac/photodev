using System;
using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16.Demosaic
{
    public interface IDebayer<in T, TC> : IRawToColorMap16Filter<T, TC>
        where T : RawMap<TC>
        where TC : IComparable<TC>
    {
    }

    public interface IBGGRDebayer<T> : IDebayer<RawBGGRMap<T>, T> where T : IComparable<T>
    {
    }
}