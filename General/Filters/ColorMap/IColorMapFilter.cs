using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    internal interface IColorMapFilter : IFilter
    {
        ColorImageFile Process(ColorImageFile image);
    }
}