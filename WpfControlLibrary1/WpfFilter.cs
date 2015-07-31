using System.IO;

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