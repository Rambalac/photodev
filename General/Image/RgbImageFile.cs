namespace com.azi.image
{
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
}