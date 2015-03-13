using System.Collections.Generic;
using com.azi.Image;

namespace com.azi.Filters
{
    class FiltersPipeline
    {
        private List<IFilter> _filters;

        public FiltersPipeline(IEnumerable<IFilter> filters)
        {
            _filters = new List<IFilter>(filters);
        }

        public RGB8Map ColorMapToRGB(ColorMap<ushort> map)
        {
            RGB8Map result=new RGB8Map(map.Width, map.Height, )
        }
    }
}
