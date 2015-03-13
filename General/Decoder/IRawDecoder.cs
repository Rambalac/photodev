using System;
using System.IO;
using com.azi.Image;

namespace com.azi.Decoder
{
    public interface IRawDecoder<T> where T : IComparable<T>
    {
        RawImageFile<T> Decode(Stream stream);
    }
}