using System.IO;
using com.azi.Decoder;
using com.azi.image;

namespace com.azi.WpfFilters
{
    interface WpfFilter
    {

    }

    abstract class AWpfRawDecoder : WpfFilter, IRawDecoder
    {
        IRawDecoder Decoder { get; set; }
        public abstract RawImageFile Decode(Stream stream);
    }
}
