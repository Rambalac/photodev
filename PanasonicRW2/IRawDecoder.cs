using com.azi.image;
using com.azi.tiff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace com.azi.decoder.panasonic
{
    interface IRawDecoder
    {
        RawImageFile Decode(Stream stream);
    }
}
