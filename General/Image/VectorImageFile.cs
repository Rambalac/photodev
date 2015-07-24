namespace com.azi.Image
{
    public class VectorImageFile : ImageFile
    {
        public VectorMap Pixels { get; set; }

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