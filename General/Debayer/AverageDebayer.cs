using com.azi.image;

namespace com.azi.Debayer
{
    public interface IDebayer
    {
        ColorMap16 Debayer(RawImageFile file);
    }

    public class AverageDebayer: IDebayer
    {
        enum ColorComponent : int
        {
            R = 0,
            G = 1,
            B = 2
        }

        ColorComponent[,] componentsMap = new ColorComponent[2, 2] {
            {ColorComponent.G, ColorComponent.R},
            {ColorComponent.B,ColorComponent.G}
        };
        bool invertRB = false;

        public ColorMap16 Debayer(RawImageFile file)
        {
            var res = new ColorMap16(file.Width, file.Height);
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
                    var component = componentsMap[y % 2, x % 2];
                    if (component == ColorComponent.G)
                    {
                        c[(invertRB) ? 2 : 0] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y)) / 2);
                        c[1] = file.GetValue(y, x);
                        c[(invertRB) ? 0 : 2] = (ushort)((file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 2);
                    }
                    else if (component == ColorComponent.R)
                    {
                        c[0] = file.GetValue(y, x);
                        c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 4);
                        c[2] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) / 4);
                    }
                    else if (component == ColorComponent.B)
                    {
                        c[0] = (ushort)((file.GetValue(x - 1, y - 1) + file.GetValue(x + 1, y - 1) + file.GetValue(x - 1, y + 1) + file.GetValue(x + 1, y + 1)) / 4);
                        c[1] = (ushort)((file.GetValue(x - 1, y) + file.GetValue(x + 1, y) + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) / 4);
                        c[2] = file.GetValue(y, x);
                    }

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
