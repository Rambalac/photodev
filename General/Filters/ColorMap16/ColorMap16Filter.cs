using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    interface IColorMap16Filter : IFilter
    {
        ColorImageFile<ushort> Process(ColorImageFile<ushort> image);
    }
}
