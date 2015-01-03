using com.azi.tiff;
namespace com.azi.image
{
    public class ImageFile
    {
        public Exif Exif { get; set; }
        public Color16[,] Pixels { get; set; }

        public int Width { get { return Exif.ImageWidth; } }
        public int Height { get { return Exif.ImageHeight; } }

        public byte[] GetBytesRgb24()
        {
            throw new System.NotImplementedException();
        }

        public int GetStrideRgb24()
        {
            throw new System.NotImplementedException();
        }
    }
}

