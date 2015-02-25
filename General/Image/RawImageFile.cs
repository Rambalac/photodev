namespace com.azi.image
{
    public class RawImageFile : ImageFile
    {
        public readonly int MaxBits;
        public readonly int[,] Raw;
        private readonly int _height;
        private readonly int _width;

        public RawImageFile(int w, int h, int[,] raw, int maxBits)
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

        internal int GetValue(int x, int y)
        {
            //if (x < 0) x = 0;
            //if (y < 0) y = 0;
            //if (x >= Width) x = Width - 1;
            //if (y >= Height) y = Height - 1;
            return Raw[y, x];
        }
    }
}