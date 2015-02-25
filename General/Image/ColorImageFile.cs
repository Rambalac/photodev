namespace com.azi.image
{
    public class ColorImageFile: ImageFile
    {
        public ColorMap Pixels { get; set; }

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