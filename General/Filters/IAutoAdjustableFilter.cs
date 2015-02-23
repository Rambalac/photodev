using com.azi.image;

namespace com.azi.Filters
{
    interface IAutoAdjustableFilter
    {
        void AutoAdjust(ColorImageFile<ushort> image);
    }
}