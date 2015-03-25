using System.IO;
using com.azi.Decoder;
using com.azi.Image;

namespace com.azi.WpfFilters
{
    internal interface WpfFilter
    {
    }

    internal abstract class AWpfRawDecoder<T> : WpfFilter, IRawDecoder<T>
    {
        private IRawDecoder<T> Decoder { get; set; }
        public abstract RawImageFile<T> Decode(Stream stream);
    }
}