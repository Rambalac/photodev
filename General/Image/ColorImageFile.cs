using System;

namespace com.azi.Image
{
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
}