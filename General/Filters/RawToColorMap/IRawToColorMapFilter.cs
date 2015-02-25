using com.azi.image;

namespace com.azi.Filters.RawToColorMap
{
    public interface IRawToColorMapFilter
    {
        ColorImageFile Process(RawImageFile raw);
    }
}