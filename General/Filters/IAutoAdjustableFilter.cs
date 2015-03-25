using com.azi.Image;

namespace com.azi.Filters
{
    public interface IIAutoAdjustableFilter
    {
    }

    public interface IAutoAdjustableFilter<in T> : IIAutoAdjustableFilter where T : IColorMap
    {
        void AutoAdjust(T map);
    }
}