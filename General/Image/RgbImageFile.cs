namespace com.azi.Image
{
    public class RgbImageFile : ImageFile
    {
        public RGB8Map Pixels { get; set; }

        public override int Width
        {
            get { return Pixels.Width; }
        }

        public override int Height
        {
            get { return Pixels.Height; }
        }
    }
}