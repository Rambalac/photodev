using com.azi.Image;

namespace com.azi.Filters.RawToVectorMap
{
    public interface IRawToVectorMapFilter<in T, TC> : IFilter
        where T : RawMap<TC>
    {
        VectorMap Process(T raw);
    }
}