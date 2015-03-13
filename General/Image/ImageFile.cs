using com.azi.tiff;

namespace com.azi.Image
{
    public abstract class ImageFile
    {
        public Exif Exif;
        public abstract int Width { get; }
        public abstract int Height { get; }
    }
}