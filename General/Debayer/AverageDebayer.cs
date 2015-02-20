using com.azi.image;

namespace com.azi.Debayer
{
    public interface IDebayer
    {
        ColorMap<ushort> Debayer(RawImageFile file);
    }

    public class AverageDebayer : IDebayer
    {
        enum ColorComponent : int
        {
            R = 0,
            G = 1,
            B = 2
        }

        readonly ColorComponent[,] _componentsMap = new ColorComponent[2, 2] {
            {ColorComponent.G, ColorComponent.R},
            {ColorComponent.B,ColorComponent.G}
        };

        private const bool InvertRb = false;

        public ColorMap<ushort> Debayer(RawImageFile file)
        {
            var res = new ColorMap<ushort>(file.Width, file.Height, file.MaxBits);
            var c = new ushort[3];
            var pix = res.GetPixel(0, 0);
            for (var x = 0; x < file.Width; x++)
            {
                pix.Next();

            }
            for (var y = 1; y < file.Height - 1; y++)
            {
                pix.Next();
                for (var x = 1; x < file.Width - 1; x++)
                {
                    var component = _componentsMap[y % 2, x % 2];
                    switch (component)
                    {
                        case ColorComponent.G:
                            c[(InvertRb) ? 2 : 0] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y)) / 2);
                            c[1] = file.GetValue(y, x);
                            c[(InvertRb) ? 0 : 2] = (ushort)((file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 2);
                            break;
                        case ColorComponent.R:
                            c[0] = file.GetValue(y, x);
                            c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 4);
                            c[2] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) / 4);
                            break;
                        case ColorComponent.B:
                            c[0] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) / 4);
                            c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 4);
                            c[2] = file.GetValue(y, x);
                            break;
                    }
                    pix[0] = c[0];
                    pix[1] = c[1];
                    pix[2] = c[2];
                    pix.Next();
                }
                pix.Next();
            }
            for (var x = 0; x < file.Width; x++)
            {
                pix.Next();

            }
            return res;
        }
    }
}
