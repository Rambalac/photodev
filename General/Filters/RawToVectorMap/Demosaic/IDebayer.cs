using com.azi.Image;

namespace com.azi.Filters.RawToVectorMap.Demosaic
{
    public interface IDebayer<in T, TC> : IRawToVectorMapFilter<T, TC>
        where T : RawMap<TC>
    {
    }

    public interface IBGGRDebayer<T> : IDebayer<RawBGGRMap<T>, T>
    {
    }
}