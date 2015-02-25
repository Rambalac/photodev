using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    internal interface IColorMap16Filter : IFilter
    {
        ColorImageFile<ushort> Process(ColorImageFile<ushort> image);
    }
}