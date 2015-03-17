using System;
using System.Collections.Generic;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16;
using com.azi.Image;

namespace com.azi.Filters
{
    public class FiltersPipeline
    {
        private List<IFilter> _filters;

        public FiltersPipeline(IEnumerable<IFilter> filters)
        {
            _filters = new List<IFilter>(filters);
        }

        public RGB8Map RawMapToRGB(RawMap<ushort> map)
        {
            var indColorFilter = new List<IndependentColorComponentFilter>();
            IColorMap currentMap = map;
            foreach (var filter in _filters)
            {
                if (currentMap is RawBGGRMap<ushort>)
                {
                    if (filter is IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort>)
                    {
                        currentMap = ApplyRawBGGRToColorMapFilter((RawBGGRMap<ushort>)currentMap, (IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort>)filter);
                    }
                    else
                    {
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " + currentMap.GetType());
                    }
                }
                else if (currentMap is ColorMap<ushort>)
                {
                    if (filter is IndependentColorComponentFilter)
                    {
                        indColorFilter.Add((IndependentColorComponentFilter)filter);
                    }
                    else if (filter is IndependentColorComponentToRGBFilter)
                    {
                        currentMap = ApplyIndependentColorFiltersWithRGB((ColorMap<ushort>)currentMap,
                            indColorFilter.ToArray(), (IndependentColorComponentToRGBFilter)filter);
                    }
                    else
                    {
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " + currentMap.GetType());
                    }
                }
                else
                {
                    throw new NotSupportedException("Not supported Map: " + currentMap.GetType());
                }
            }

            return (RGB8Map)currentMap;
        }

        private static IColorMap ApplyRawBGGRToColorMapFilter(RawBGGRMap<ushort> map, IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort> filter)
        {
            return filter.Process(map);
        }

        private static RGB8Map ApplyIndependentColorFiltersWithRGB(ColorMap<ushort> map, IndependentColorComponentFilter[] indColorFilters, IndependentColorComponentToRGBFilter rgbFilter)
        {
            var result = new RGB8Map(map.Width, map.Height);
            var maxValue = map.MaxValue;
            var curve = new[] { new byte[maxValue + 1], new byte[maxValue + 1], new byte[maxValue + 1] };
            var rgb = new float[3];
            for (var i = 0; i <= maxValue; i++)
            {
                rgb[0] = i / (float)maxValue;
                rgb[1] = i / (float)maxValue;
                rgb[2] = i / (float)maxValue;

                for (var f = 0; f < indColorFilters.Length; f++)
                {
                    indColorFilters[f].ProcessColor(rgb, rgb);
                }
                curve[0][i] = rgbFilter.ProcessColor(Math.Max(0, Math.Min(1f, rgb[0])), 0);
                curve[1][i] = rgbFilter.ProcessColor(Math.Max(0, Math.Min(1f, rgb[1])), 1);
                curve[2][i] = rgbFilter.ProcessColor(Math.Max(0, Math.Min(1f, rgb[2])), 2);
            }
            for (var y = 0; y < result.Height; y++)
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[0][input.R], curve[1][input.G], curve[2][input.B]);
                    input.MoveNext();
                }
            }
            return result;
        }

        private static ColorMap<ushort> ApplyIndependentColorFilters(ColorMap<ushort> map, IndependentColorComponentFilter[] indColorFilters)
        {
            if (indColorFilters.Length == 0) return map;

            var result = new ColorMap<ushort>(map.Width, map.Height, map.MaxBits);
            var maxValue = map.MaxValue;
            var curve = new[] { new ushort[maxValue + 1], new ushort[maxValue + 1], new ushort[maxValue + 1] };
            var rgb = new float[3];
            for (var i = 0; i <= maxValue; i++)
            {
                rgb[0] = i / (float)maxValue;
                rgb[1] = i / (float)maxValue;
                rgb[2] = i / (float)maxValue;
                for (var f = 0; f < indColorFilters.Length; f++)
                {
                    indColorFilters[f].ProcessColor(rgb, rgb);
                }

                curve[0][i] = (ushort)(maxValue * Math.Max(0, Math.Min(1f, rgb[0])));
                curve[1][i] = (ushort)(maxValue * Math.Max(0, Math.Min(1f, rgb[1])));
                curve[2][i] = (ushort)(maxValue * Math.Max(0, Math.Min(1f, rgb[2])));
            }
            for (var y = 0; y < result.Height; y++)
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[0][input.R], curve[1][input.G], curve[2][input.B]);
                    input.MoveNext();
                }
            }
            return result;
        }

        public void AutoAjust(RawMap<ushort> map, IAutoAdjustableFilter autofilter)
        {
            var indColorFilter = new List<IndependentColorComponentFilter>();
            IColorMap currentMap = map;
            foreach (var filter in _filters)
            {
                if (filter == autofilter)
                {
                    var result = ApplyIndependentColorFilters((ColorMap<ushort>)currentMap,
                        indColorFilter.ToArray());
                    autofilter.AutoAdjust(result);
                    return;
                }
                if (currentMap is RawBGGRMap<ushort>)
                {
                    if (filter is IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort>)
                    {
                        currentMap = ApplyRawBGGRToColorMapFilter((RawBGGRMap<ushort>)currentMap,
                            (IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort>)filter);
                    }
                    else
                    {
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " +
                                                        currentMap.GetType());
                    }
                }
                else if (currentMap is ColorMap<ushort>)
                {
                    if (filter is IndependentColorComponentFilter)
                    {
                        indColorFilter.Add((IndependentColorComponentFilter)filter);
                    }
                    else if (filter is IndependentColorComponentToRGBFilter)
                    {
                        currentMap = ApplyIndependentColorFiltersWithRGB((ColorMap<ushort>)currentMap,
                            indColorFilter.ToArray(), (IndependentColorComponentToRGBFilter)filter);
                    }
                    else
                    {
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " +
                                                        currentMap.GetType());
                    }
                }
                else
                {
                    throw new NotSupportedException("Not supported Map: " + currentMap.GetType());
                }
            }

        }
    }

}
