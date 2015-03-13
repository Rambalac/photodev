using com.azi.Debayer;
using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16
{
    public class DebayerFilter : IRawToColorMap16Filter
    {
        public IDebayer<RawMap<ushort>> Debayer { set; get; }

        public ColorMap<ushort> Process(RawMap<ushort> raw)
        {
            return Debayer.Debayer(raw);
        }
    }
}