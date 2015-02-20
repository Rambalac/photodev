using com.azi.image;

namespace com.azi.Debayer
{
    public class AverageDebayer : IDebayer
    {
        enum ColorComponent : int
        {
            R = 0,
            G = 1,
            B = 2
        }

        readonly ColorComponent[,] _componentsMap =
        {
            {ColorComponent.B, ColorComponent.G},
            {ColorComponent.G,ColorComponent.R}
        };

        //{
        //    {ColorComponent.G, ColorComponent.R},
        //    {ColorComponent.B,ColorComponent.G}
        //};

        public ColorMap<ushort> Debayer(RawImageFile file)
        {
            //var res = new ColorMap<ushort>(file.Width, file.Height, file.MaxBits);
            var res = new ColorMap<ushort>(600, 600, file.MaxBits + 2);
            var c = new ushort[3];
            var pix = res.GetPixel();
            for (var x = 0; x < res.Width; x++)
            {
                pix.Next();

            }
            for (var y = 1; y < res.Height - 1; y++)
            {
                pix.Next();
                for (var x = 1; x < res.Width - 1; x++)
                {
                    var component = _componentsMap[(y+1) % 2, (x+0) % 2];
                    c[0] = 0;
                    c[1] = 0;
                    c[2] = 0;
                    var invertRb = ((x % 2) ^ (y % 2)) == 0;
                    switch (component)
                    {
                        case ColorComponent.R:
                            c[0] = (ushort)(file.GetValue(x, y) >> 2);
                            c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) );
                            c[2] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) );
                            break;
                        case ColorComponent.G:
                            //c[(invertRb) ? 2 : 0] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y)) >>1);
                            c[1] = (ushort)(file.GetValue(x, y) >> 2);
                            //c[(invertRb) ? 0 : 2] = (ushort)((file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) >>1);
                            break;
                        case ColorComponent.B:
                            c[0] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) );
                            c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) );
                            c[2] = (ushort)(file.GetValue(x, y) >> 2);
                            break;
                    }
                    pix[0] = c[0];
                    pix[1] = c[1];
                    pix[2] = c[2];
                    pix.Next();
                }
                pix.Next();
            }
            for (var x = 0; x < res.Width; x++)
            {
                pix.Next();

            }
            return res;
        }
    }
}
