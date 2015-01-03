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

        /*unsigned CLASS pana_bits (int nbits)
        {
            static uchar buf[0x4000];
            static int vbits;
            int byte;

            if (!nbits) return vbits=0;
            if (!vbits) {
            fread (buf+load_flags, 1, 0x4000-load_flags, ifp);
            fread (buf, 1, load_flags, ifp);
            }
            vbits = (vbits - nbits) & 0x1ffff;
            byte = vbits >> 3 ^ 0x3ff0;
            return (buf[byte] | buf[byte+1] << 8) >> (vbits & 7) & ~(-1 << nbits);
        }

        void CLASS panasonic_load_raw()
        {
            int row, col, i, j, sh=0, pred[2], nonz[2];

            pana_bits(0);
            for (row=0; row < height; row++)
            for (col=0; col < raw_width; col++) {
                if ((i = col % 14) == 0)
	        pred[0] = pred[1] = nonz[0] = nonz[1] = 0;
                if (i % 3 == 2) sh = 4 >> (3 - pana_bits(2));
                if (nonz[i & 1]) {
	        if ((j = pana_bits(8))) {
	            if ((pred[i & 1] -= 0x80 << sh) < 0 || sh == 4)
	                pred[i & 1] &= ~(-1 << sh);
	            pred[i & 1] += j << sh;
	        }
                } else if ((nonz[i & 1] = pana_bits(8)) || i > 11)
	        pred[i & 1] = nonz[i & 1] << 4 | pana_bits(4);
                if ((RAW(row,col) = pred[col & 1]) > 4098 && col < width) derror();
            }
        }*/
 
        public ImageFile decode(Stream stream)
        {
            PanasonicExif exif = PanasonicExif.parse(stream);
            ImageFile result=new ImageFile();

            return result;
        }

    }
}
