namespace com.azi.Image
{
    public class RawImageFile<T> : ImageFile
    {
        public readonly RawMap<T> Raw;

        public RawImageFile(RawMap<T> raw)
        {
            Raw = raw;
        }

        public override int Width
        {
            get { return Raw.Width; }
        }

        public override int Height
        {
            get { return Raw.Height; }
        }

        //internal ushort GetValue(int x, int y)
        //{
        //    //if (x < 0) x = 0;
        //    //if (y < 0) y = 0;
        //    //if (x >= Width) x = Width - 1;
        //    //if (y >= Height) y = Height - 1;
        //    return Raw[y, x];
        //}
    }
}