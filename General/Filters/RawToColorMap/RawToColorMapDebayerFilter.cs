using com.azi.Debayer;
using com.azi.image;

namespace com.azi.Filters.RawToColorMap
{
    public class DebayerFilter : IRawToColorMapFilter
    {
        public IDebayer Debayer { set; get; }

        public ColorImageFile Process(RawImageFile raw)
        {
            return new ColorImageFile
            {
                Exif = raw.Exif,
                Pixels = Debayer.Debayer(raw)
            };
        }
    }
}