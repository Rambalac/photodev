using com.azi.Image;

namespace com.azi.Filters
{
    public interface IAutoAdjustableFilter
    {
        void AutoAdjust(ColorMap<ushort> map);
    }
}