using com.azi.image;

namespace com.azi.Filters
{
    internal interface IAutoAdjustableFilter
    {
        void AutoAdjust(ColorImageFile image);
    }
}