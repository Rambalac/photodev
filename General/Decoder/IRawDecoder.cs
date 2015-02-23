using System.IO;
using com.azi.image;

namespace com.azi.Decoder
{
    public interface IRawDecoder
    {
        RawImageFile Decode(Stream stream);
    }
}