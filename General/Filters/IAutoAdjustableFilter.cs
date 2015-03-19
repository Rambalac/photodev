using com.azi.Image;

namespace com.azi.Filters
{
    public interface IAutoAdjustableFilter<T> where T : IColorMap
    {
        void AutoAdjust(T map);
    }
}