using System;
using System.IO;
using com.azi.Decoder;
using com.azi.Image;

namespace com.azi.WpfFilters
{
    interface WpfFilter
    {

    }

    abstract class AWpfRawDecoder<T> : WpfFilter, IRawDecoder<T> where T : IComparable<T>
    {
        IRawDecoder<T> Decoder { get; set; }
        public abstract RawImageFile<T> Decode(Stream stream);
    }
}
