using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.azi.image;

namespace com.azi.Debayer
{
    public class BinningDebayer : IDebayer
    {
        public ColorMap<ushort> Debayer(RawImageFile file)
        {
            var res = new ColorMap<ushort>(file.Width / 2, file.Height / 2, file.MaxBits);
            var c = new ushort[4];
            var pix = res.GetPixel();
            for (var y = 0; y < res.Height; y++)
            {
                for (var x = 0; x < res.Width; x++)
                {
                    c[0] = file.GetValue(x * 2 + 0, y * 2 + 0);
                    c[1] = file.GetValue(x * 2 + 1, y * 2 + 0);
                    c[2] = file.GetValue(x * 2 + 0, y * 2 + 1);
                    c[3] = file.GetValue(x * 2 + 1, y * 2 + 1);

                    pix[0] = c[3];
                    pix[1] = c[1];
                    pix[2] = c[0];
                    pix.Next();
                }
            }
            return res;
        }
    }
}
