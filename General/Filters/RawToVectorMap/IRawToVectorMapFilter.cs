using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16
{
    public interface IRawToVectorMapFilter<in T, TC> : IFilter
        where T : RawMap<TC>
    {
        VectorMap Process(T raw);
    }
}