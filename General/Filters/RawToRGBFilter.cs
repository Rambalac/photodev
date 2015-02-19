using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.azi.Debayer;
using com.azi.image;

namespace com.azi.Filters
{
    public class RawToRgbFilter
    {
        public IDebayer Debayer { set; get; }

        public Rgb16ImageFile Process(RawImageFile raw)
        {
            return new Rgb16ImageFile
            {
                Exif = raw.Exif,
                Height = raw.Height,
                Width = raw.Width,
                Pixels = Debayer.Debayer(raw)
            };
        }
    }
}
