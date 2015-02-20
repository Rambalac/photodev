using com.azi.tiff;
namespace com.azi.image
{
    public abstract class ImageFile
    {
        public Exif Exif;
        public abstract int Width { get; }
        public abstract int Height { get; }
    }

    public class ColorImageFile<T> : ImageFile
    {
        public ColorMap<T> Pixels { get; set; }

        public override int Width
        {
            get { return Pixels.Width; }
        }

        public override int Height
        {
            get { return Pixels.Height; }
        }
    }

    public class RgbImageFile : ImageFile
    {
        public Rgb8Map Pixels { get; set; }

        public override int Width
        {
            get { return Pixels.Width; }
        }

        public override int Height
        {
            get { return Pixels.Height; }
        }
    }

    public class RawImageFile : ImageFile
    {
        private readonly int _width;
        private readonly int _height;
        public readonly int MaxBits;
        public readonly ushort[,] Raw;

        public RawImageFile(int w, int h, ushort[,] raw, int maxBits)
        {
            Raw = raw;
            _width = w;
            _height = h;
            MaxBits = maxBits;
        }

        public override int Width
        {
            get { return _width; }
        }

        public override int Height
        {
            get { return _height; }
        }

        internal ushort GetValue(int x, int y)
        {
            //if (x < 0) x = 0;
            //if (y < 0) y = 0;
            //if (x >= Width) x = Width - 1;
            //if (y >= Height) y = Height - 1;
            return Raw[y, x];
        }
    }

}

