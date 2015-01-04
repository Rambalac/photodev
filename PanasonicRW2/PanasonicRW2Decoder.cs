using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.azi.image;
using com.azi.tiff;
using con.azi.decoder.panasonic.rw2;

namespace com.azi.decoder.panasonic.rw2
{
    public class PanasonicRW2Decoder
    {
        class BitStream
        {
            const int bufsize = 16384;
            byte[] buf = new byte[bufsize + 1];
            int vbits;
            Stream stream;
            int load_flags = 8200;

            public BitStream(Stream stream)
            {
                this.stream = stream;
            }

            public int read(int nbits)
            {
                unchecked
                {
                    if (vbits == 0)
                    {
                        stream.Read(buf, load_flags, bufsize - load_flags);
                        stream.Read(buf, 0, load_flags);
                    }
                    vbits = (vbits - nbits) & 0x1ffff;
                    var bytepos = vbits >> 3 ^ 0x3ff0;

                    return ((((int)buf[bytepos]) | ((int)buf[bytepos + 1]) << 8) >> (vbits & 7)) & (~(-1 << nbits));
                }
            }
        }

        RawImageFile loadRaw(Stream stream, PanasonicExif exif)
        {
            int row, col, i, j, sh = 0;
            int[] pred = new int[2], nonz = new int[2];

            ushort[,] raw = new ushort[exif.ImageHeight, exif.ImageWidth];

            BitStream bits = new BitStream(stream);
            for (row = 0; row < exif.ImageHeight; row++)
                for (col = 0; col < exif.ImageWidth; col++)
                    unchecked
                    {
                        i = col % 14;
                        if (i == 0)
                            pred[0] = pred[1] = nonz[0] = nonz[1] = 0;
                        if (i % 3 == 2)
                            sh = 4 >> (3 - bits.read(2));
                        if (nonz[i & 1] != 0)
                        {
                            j = bits.read(8);
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
                            nonz[i & 1] = bits.read(8);
                            if (nonz[i & 1] != 0 || i > 11)
                                pred[i & 1] = nonz[i & 1] << 4 | bits.read(4);
                        }
                        raw[row, col] = (ushort)pred[col & 1];

                        if (raw[row, col] > 4098 && col < exif.CropRight)
                            throw new Exception("Decoding error");
                    }
            RawImageFile result = new RawImageFile
            {
                Exif = exif,
                Height = exif.ImageHeight,
                Width = exif.ImageWidth,
                Raw = raw
            };
            return result;
        }

        public ImageFile decode(Stream stream)
        {
            PanasonicExif exif = PanasonicExif.parse(stream);

            stream.Seek(exif.RawOffset, SeekOrigin.Begin);

            return loadRaw(stream, exif);
        }

    }
}
