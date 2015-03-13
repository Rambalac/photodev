using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    internal interface IColorMap16Filter : IFilter
    {
        ColorImageFile<ushort> Process(ColorImageFile<ushort> image);
    }
}