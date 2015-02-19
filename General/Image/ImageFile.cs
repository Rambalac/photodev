using com.azi.tiff;
namespace com.azi.image
{
    public abstract class ImageFile
    {
        public Exif Exif;
        public int Width;
        public int Height;
    }

    public class Rgb16ImageFile : ImageFile
    {
        public ColorMap16 Pixels { get; set; }
        public byte[] GetBytesRgb8()
        {

            return null;
        }

        public int GetStrideRgb8()
        {
            return 0;
        }
    }
    public class RawImageFile : ImageFile
    {
        public ushort[,] Raw;

        internal ushort GetValue(int y, int x)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x >= Width) x = Width - 1;
            if (y >= Height) y = Height - 1;
            return Raw[y, x];
        }
    }

}

