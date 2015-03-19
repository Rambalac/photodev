using com.azi.Image;

namespace com.azi.Filters.RawToColorMap16.Demosaic
{
    public class AverageDebayer : IDebayer<RawMap<ushort>, ushort>
    {
        private readonly ColorComponent[,] _componentsMap =
        {
            {ColorComponent.B, ColorComponent.G},
            {ColorComponent.G, ColorComponent.R}
        };

        //{
        //    {ColorComponent.G, ColorComponent.B},
        //    {ColorComponent.R,ColorComponent.G}
        //};


        public ColorMap<ushort> Process(RawMap<ushort> map)
        {
            var res = new ColorMapUshort(map.Width, map.Height, map.MaxBits + 2);
            var pix = res.GetPixel();
            var file = map.GetPixel();
            for (var x = 0; x < res.Width; x++)
            {
                pix.MoveNext();
                file.MoveNext();
            }
            for (var y = 1; y < res.Height - 1; y++)
            {
                pix.MoveNext();
                file.MoveNext();
                for (var x = 1; x < res.Width - 1; x++)
                {
                    var component = _componentsMap[(y + 0) % 2, (x + 0) % 2];
                    var invertRb = ((x % 2) ^ (y % 2)) == 0;
                    switch (component)
                    {
                        case ColorComponent.R:
                            pix[0] = (ushort)(file.Value << 2);
                            pix[1] = (ushort)((file.GetRel(-1, 0) + file.GetRel(+1, 0) + file.GetRel(0, -1) + file.GetRel(0, +1)));
                            pix[2] = (ushort)((file.GetRel(-1, -1) + file.GetRel(+1, -1) + file.GetRel(-1, +1) + file.GetRel(+1, +1)));
                            break;
                        case ColorComponent.G:
                            pix[(invertRb) ? 2 : 0] = (ushort)((file.GetRel(-1, 0) + file.GetRel(+1, 0)) << 1);
                            pix[1] = (ushort)(file.Value << 2);
                            pix[(invertRb) ? 0 : 2] = (ushort)((file.GetRel(0, -1) + file.GetRel(0, +1)) << 1);
                            break;
                        case ColorComponent.B:
                            pix[0] = (ushort)((file.GetRel(-1, -1) + file.GetRel(+1, -1) + file.GetRel(-1, +1) + file.GetRel(+1, +1)));
                            pix[1] = (ushort)((file.GetRel(-1, 0) + file.GetRel(+1, 0) + file.GetRel(0, -1) + file.GetRel(0, +1)));
                            pix[2] = (ushort)(file.Value << 2);
                            break;
                    }
                    pix.MoveNext();
                    file.MoveNext();
                }
                pix.MoveNext();
                file.MoveNext();
            }
            for (var x = 0; x < res.Width; x++)
            {
                pix.MoveNext();
                file.MoveNext();
            }
            return res;
        }

        private enum ColorComponent
        {
            R = 0,
            G = 1,
            B = 2
        }
    }
}