using com.azi.image;

namespace com.azi.Filters.ColorMapToRgb8
{
    public interface IColorMapToRgb8Filter : IFilter
    {
        RgbImageFile Process(ColorImageFile image);
    }
}