using com.azi.Image;

namespace com.azi.Filters
{
    internal interface IAutoAdjustableFilter
    {
        void AutoAdjust(ColorMap<ushort> map);
    }
}