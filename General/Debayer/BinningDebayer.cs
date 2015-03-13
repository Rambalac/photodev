using com.azi.Image;

namespace com.azi.Debayer
{
    public class BinningBGGRDebayer : IDebayer<RawMap<ushort>>
    {
        public ColorMap<ushort> Debayer(RawMap<ushort> map)
        {
            var res = new ColorMap<ushort>(map.Width / 2, map.Height / 2, map.MaxBits);
            var c = new ushort[4];
            var pix = res.GetPixel();
            for (var y = 0; y < res.Height; y++)
            {
                var raw = map.GetRow(y * 2);
                for (var x = 0; x < res.Width; x++)
                {
                    c[0] = raw.Value;
                    c[1] = raw.GetRel(+1, +0);
                    c[2] = raw.GetRel(0, +1);
                    c[3] = raw.GetRel(+1, +1);

                    pix[0] = c[3];
                    pix[1] = c[1];
                    pix[2] = c[0];
                    pix.MoveNext();
                    raw.MoveNext();
                    raw.MoveNext();
                }
            }
            return res;
        }
    }
}