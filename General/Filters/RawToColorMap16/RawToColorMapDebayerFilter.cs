using com.azi.Debayer;
using com.azi.image;

namespace com.azi.Filters.RawToColorMap16
{
    public class DebayerFilter : IRawToColorMap16Filter
    {
        public IDebayer Debayer { set; get; }

        public ColorImageFile<ushort> Process(RawImageFile raw)
        {
            return new ColorImageFile<ushort>
            {
                Exif = raw.Exif,
                Pixels = Debayer.Debayer(raw)
            };
        }
    }
}