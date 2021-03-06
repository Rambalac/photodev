﻿using System;
using System.IO;
using com.azi.Image;

namespace com.azi.Decoder.Panasonic.Rw2
{
    /// <summary>
    ///     Panasonic RW2 file decoder
    ///     Thanks to dcraw
    /// </summary>
    public class PanasonicRW2Decoder : IRawDecoder<ushort>
    {
        public RawImageFile<ushort> Decode(Stream stream)
        {
            var exif = PanasonicExif.Parse(stream);

            stream.Seek(exif.RawOffset, SeekOrigin.Begin);

            return DecodeImagePart(stream, exif);
        }

        private static RawImageFile<ushort> DecodeImagePart(Stream stream, PanasonicExif exif)
        {
            int row, col, i, j, sh = 0;
            int[] pred = new int[2], nonz = new int[2];

            var resultHeight = exif.ImageHeight;
            var resultWidth = exif.CropRight;
            var map = new RawBGGRMap<ushort>(resultWidth, resultHeight, 12);
            var raw = map.GetPixel();
            int value;
            var bits = new PanasonicBitStream(stream);
            for (row = 0; row < exif.ImageHeight; row++)
                for (col = 0; col < exif.ImageWidth; col++)
                    unchecked
                    {
                        i = col%14;
                        if (i == 0)
                            pred[0] = pred[1] = nonz[0] = nonz[1] = 0;
                        if (i%3 == 2)
                            sh = 4 >> (3 - bits.Read(2));
                        if (nonz[i & 1] != 0)
                        {
                            j = bits.Read(8);
                            if (j != 0)
                            {
                                pred[i & 1] -= 0x80 << sh;
                                if (pred[i & 1] < 0 || sh == 4)
                                    pred[i & 1] &= ~(-1 << sh);
                                pred[i & 1] += j << sh;
                            }
                        }
                        else
                        {
                            nonz[i & 1] = bits.Read(8);
                            if (nonz[i & 1] != 0 || i > 11)
                                pred[i & 1] = nonz[i & 1] << 4 | bits.Read(4);
                        }
                        if (col >= resultWidth) continue;

                        value = pred[col & 1];

                        if (value > 4098)
                            throw new Exception("Decoding error");

                        raw.SetAndMoveNext((ushort) Math.Min(4095, value));
                    }
            var result = new RawImageFile<ushort>(map)
            {
                Exif = exif,
            };
            return result;
        }

        /// <summary>
        ///     Panasonic specific bit stream
        /// </summary>
        private class PanasonicBitStream
        {
            private const int Bufsize = 16384;
            private const int LoadFlags = 8200;
            private readonly byte[] _buf = new byte[Bufsize + 1];
            private readonly Stream _stream;
            private int _bitsLeft;

            public PanasonicBitStream(Stream stream)
            {
                _stream = stream;
            }

            /// <summary>
            ///     Reads up to 16 bits
            /// </summary>
            /// <param name="numberOfBits">Number of bots to read</param>
            /// <returns>int with required number of bits on low part</returns>
            public int Read(int numberOfBits)
            {
                unchecked
                {
                    if (_bitsLeft == 0)
                    {
                        _stream.Read(_buf, LoadFlags, Bufsize - LoadFlags);
                        _stream.Read(_buf, 0, LoadFlags);
                    }
                    _bitsLeft = (_bitsLeft - numberOfBits) & 0x1ffff;
                    var bytepos = _bitsLeft >> 3 ^ 0x3ff0;

                    return ((_buf[bytepos] | _buf[bytepos + 1] << 8) >> (_bitsLeft & 7)) & (~(-1 << numberOfBits));
                }
            }
        }
    }
}