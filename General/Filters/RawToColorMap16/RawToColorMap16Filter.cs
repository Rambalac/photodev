using com.azi.image;

namespace com.azi.Filters.RawToColorMap16
{
    public interface IRawToColorMap16Filter
    {
        ColorImageFile<ushort> Process(RawImageFile raw);
    }
}