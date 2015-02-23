﻿using com.azi.Compressor;
using com.azi.image;

namespace com.azi.Filters.ColorMap16ToRgb8
{
    public class CompressorFilter : IColorMap16ToRgb8Filter
    {
        public ICompressor Compressor { set; get; }

        public RgbImageFile Process(ColorImageFile<ushort> image)
        {
            return new RgbImageFile
            {
                Exif = image.Exif,
                Pixels = Compressor.Compress(image.Pixels, 4)
            };
        }
    }
}