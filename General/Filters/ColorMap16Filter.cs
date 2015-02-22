using com.azi.image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.azi.Filters
{
    interface IAutoAdjustableFilter
    {
        void AutoAdjust(ColorImageFile<ushort> image);
    }
    interface IColorMap16Filter : IFilter
    {
        ColorImageFile<ushort> Process(ColorImageFile<ushort> image);
    }
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
