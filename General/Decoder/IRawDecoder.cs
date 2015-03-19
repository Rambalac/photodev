using System;
using System.IO;
using com.azi.Image;

namespace com.azi.Decoder
{
    public interface IRawDecoder<T>
    {
        RawImageFile<T> Decode(Stream stream);
    }
}