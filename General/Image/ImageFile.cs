using com.azi.tiff;
namespace com.azi.image
{
    public abstract class ImageFile
    {
        public Exif Exif;
        public int Width;
        public int Height;

        abstract public byte[] GetBytesRgb24();

        abstract public int GetStrideRgb24();
    }

    public class RgbImageFile : ImageFile
    {
        public Color16[,] Pixels { get; set; }
        override public byte[] GetBytesRgb24()
        {

            return null;
        }

        override public int GetStrideRgb24()
        {
            return 0;
        }
    }
    public class RawImageFile : RgbImageFile
    {
        public ushort[,] Raw;
    }

}

