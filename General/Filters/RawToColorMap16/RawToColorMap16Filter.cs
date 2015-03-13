using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16
{
    public interface IRawToColorMap16Filter
    {
        ColorMap<ushort> Process(RawMap<ushort> raw);
    }
}