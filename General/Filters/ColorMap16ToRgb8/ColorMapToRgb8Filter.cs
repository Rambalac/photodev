using com.azi.image;

namespace com.azi.Filters.ColorMap16ToRgb8
{
    public interface IColorMap16ToRgb8Filter : IFilter
    {
        RgbImageFile Process(ColorImageFile<ushort> image);
    }
}