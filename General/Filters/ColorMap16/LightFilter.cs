using System;
using com.azi.image;

namespace com.azi.Filters.ColorMap16
{
    class LightFilter : IColorMap16Filter, IAutoAdjustableFilter
    {
        public ColorImageFile<ushort> Process(ColorImageFile<ushort> image)
        {
            throw new NotImplementedException();
        }

        public void AutoAdjust(ColorImageFile<ushort> image)
        {
            throw new NotImplementedException();
        }
    }
}