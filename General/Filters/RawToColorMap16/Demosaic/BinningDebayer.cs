using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16.Demosaic
{
    public class BinningBGGRDebayer : IDebayer<RawMap<ushort>, ushort>
    {
        public ColorMap<ushort> Process(RawMap<ushort> map)
        {
            var res = new ColorMapUshort(map.Width/2, map.Height/2, map.MaxBits);
            var c = new ushort[4];
            Color<ushort> pix = res.GetPixel();
            for (int y = 0; y < res.Height; y++)
            {
                RawPixel<ushort> raw = map.GetRow(y*2);
                for (int x = 0; x < res.Width; x++)
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