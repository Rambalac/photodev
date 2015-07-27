using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.azi.Filters.RawToColorMap16;
using com.azi.Image;
using System.Numerics;

namespace com.azi.Filters
{
    public class FiltersPipeline
    {
        private readonly List<IFilter> _filters;

        public FiltersPipeline(IEnumerable<IFilter> filters)
        {
            _filters = new List<IFilter>(filters);
        }

        public RGB8Map RawMapToRGB(RawMap<ushort> map)
        {
            var currentMap = ProcessFilters(map);

            return (RGB8Map)currentMap;
        }

        private IColorMap ProcessFilters(RawMap<ushort> map, IIAutoAdjustableFilter autoFilter = null)
        {
            var indColorFilter = new List<IndependentComponentColorToColorFilter<float, float>>();
            var indVectorFilter = new List<IndependentComponentVectorToVectorFilter>();
            IColorMap currentMap = map;
            foreach (var filter in _filters)
            {
#if DEBUG
                if (currentMap is ColorMapFloat)
                {
                    var m = currentMap as ColorMapFloat;
                    if (m.Rgb.Any(float.IsNaN)) throw new Exception();
                }
#endif
                if (filter == null)
                    throw new ArgumentNullException("filter");

                if (filter == autoFilter)
                {
                    if (filter is IAutoAdjustableFilter<ColorMapFloat>)
                    {
                        if (currentMap is ColorMapUshort)
                        {
                            currentMap = ApplyIndependentColorToFloatFilters((ColorMapUshort)currentMap,
                                indColorFilter);
                        }
                        ((IAutoAdjustableFilter<ColorMapFloat>)autoFilter).AutoAdjust((ColorMapFloat)currentMap);
                    }
                    else  if (filter is IAutoAdjustableFilter<VectorMap>)
                    {
                        if (currentMap is ColorMapUshort)
                        {
                            currentMap = ApplyIndependentColorToVectorFilters((ColorMapUshort)currentMap,
                                indVectorFilter);
                        }
                        ((IAutoAdjustableFilter<VectorMap>)autoFilter).AutoAdjust((VectorMap)currentMap);
                    }
                    else
                        throw new NotSupportedException("Not supported AutoAdjust Filter: " + filter.GetType() +
                                                        " for Map: " + currentMap.GetType());
                    return currentMap;
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
                else if (currentMap is ColorMapUshort)
                {
                    if (filter is IndependentComponentColorToColorFilter<float, float>)
                    {
                        indColorFilter.Add((IndependentComponentColorToColorFilter<float, float>)filter);
                    }
                    else if (filter is IndependentComponentVectorToVectorFilter)
                    {
                        indVectorFilter.Add((IndependentComponentVectorToVectorFilter)filter);
                    }
                    else if (filter is ColorToColorFilter<float, byte>)
                    {
                        currentMap = ApplyIndependentColorFiltersWithRGB((ColorMapUshort)currentMap,
                            indColorFilter, (IndependentComponentColorToColorFilter<float, byte>)filter);
                    }
                    else if (filter is ColorToColorFilter<float, float>)
                    {
                        currentMap = ApplyIndependentColorToFloatFilters((ColorMapUshort)currentMap,
                            indColorFilter);
                        ApplySingleFilterInplace((ColorMap<float>)currentMap, (ColorToColorFilter<float, float>)filter);
                    }
                    else if (filter is VectorToColorFilter<byte>)
                    {
                        currentMap = ApplyIndependentVectorFiltersWithRGB((ColorMapUshort)currentMap,
                            indVectorFilter, (IndependentComponentVectorToColorFilter<byte>)filter);
                    }
                    else if (filter is VectorToVectorFilter)
                    {
                        currentMap = ApplyIndependentColorToVectorFilters((ColorMapUshort)currentMap,
                            indVectorFilter);
                        ApplySingleFilterInplace((VectorMap)currentMap, (VectorToVectorFilter)filter);
                    }
                    else
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " +
                                                        currentMap.GetType());
                }
                else if (currentMap is ColorMapFloat)
                {
                    if (filter is ColorToColorFilter<float, float>)
                    {
                        ApplySingleFilterInplace((ColorMapFloat)currentMap, (ColorToColorFilter<float, float>)filter);
                    }
                    else if (filter is ColorToColorFilter<float, byte>)
                    {
                        currentMap = ConvertToRGB((ColorMapFloat)currentMap, (ColorToColorFilter<float, byte>)filter);
                    }
                    else
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " +
                                                        currentMap.GetType());
                }
                else if (currentMap is VectorMap)
                {
                    if (filter is VectorToVectorFilter)
                    {
                        ApplySingleFilterInplace((VectorMap)currentMap, (VectorToVectorFilter)filter);
                    }
                    else if (filter is VectorToColorFilter<byte>)
                    {
                        currentMap = ConvertToRGB((VectorMap)currentMap, (VectorToColorFilter<byte>)filter);
                    }
                    else
                        throw new NotSupportedException("Not supported Filter: " + filter.GetType() + " for Map: " +
                                                        currentMap.GetType());
                }
                else
                {
                    throw new NotSupportedException("Not supported Map: " + currentMap.GetType());
                }
            }
            return currentMap;
        }

        public void AutoAdjust(RawMap<ushort> map, IIAutoAdjustableFilter autoFilter)
        {
            ProcessFilters(map, autoFilter);
        }


        private static void ApplySingleFilterInplace(ColorMap<float> map, ColorToColorFilter<float, float> filter)
        {
            Parallel.For(0, map.Height, y =>
            {
                var pix = map.GetRow(y);
                for (var x = 0; x < map.Width; x++)
                {
                    filter.ProcessColor(pix, pix);
                    pix.MoveNext();
                }
            });
        }

        private static void ApplySingleFilterInplace(VectorMap map, VectorToVectorFilter filter)
        {
            Parallel.For(0, map.Height, y =>
            {
                var pix = map.GetRow(y);
                for (var x = 0; x < map.Width; x++)
                {
                    filter.ProcessColor(pix, pix);
                    pix.MoveNext();
                }
            });
        }

        private static RGB8Map ConvertToRGB(ColorMap<float> map, ColorToColorFilter<float, byte> filter)
        {
            var result = new RGB8Map(map.Width, map.Height);
            Parallel.For(0, map.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < map.Width; x++)
                {
                    filter.ProcessColor(input, output);
                    input.MoveNext();
                    output.MoveNext();
                }
            });
            return result;
        }
        private static RGB8Map ConvertToRGB(VectorMap map, VectorToColorFilter<byte> filter)
        {
            var result = new RGB8Map(map.Width, map.Height);
            Parallel.For(0, map.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < map.Width; x++)
                {
                    filter.ProcessColor(input, output);
                    input.MoveNext();
                    output.MoveNext();
                }
            });
            return result;
        }
        private static IColorMap ApplyRawBGGRToColorMapFilter(RawBGGRMap<ushort> map,
            IRawToColorMap16Filter<RawBGGRMap<ushort>, ushort> filter)
        {
            return filter.Process(map);
        }

        private static RGB8Map ApplyIndependentColorFiltersWithRGB(ColorMapUshort map,
            ICollection<IndependentComponentColorToColorFilter<float, float>> indColorFilters,
            IndependentComponentColorToColorFilter<float, byte> colorFilter)
        {
            if (!indColorFilters.Any()) throw new NotSupportedException("Is not supported without filters");

            var maxValue = map.MaxValue;
            var curve = new[] { new byte[maxValue + 1], new byte[maxValue + 1], new byte[maxValue + 1] };
            var curvef = ConvertToCurveF(indColorFilters, maxValue);

            Parallel.For(0, maxValue + 1, i => colorFilter.ProcessColorInCurve(i, curvef, curve));

            indColorFilters.Clear();
            return ApplyCurve(map, curve);
        }

        private static RGB8Map ApplyIndependentVectorFiltersWithRGB(ColorMapUshort map,
            ICollection<IndependentComponentVectorToVectorFilter> indColorFilters,
            IndependentComponentVectorToColorFilter<byte> colorFilter)
        {
            if (!indColorFilters.Any()) throw new NotSupportedException("Is not supported without filters");

            var maxValue = map.MaxValue;
            var curve = new[] { new byte[maxValue + 1], new byte[maxValue + 1], new byte[maxValue + 1] };
            var curvef = ConvertToCurveV(indColorFilters, maxValue);

            Parallel.For(0, maxValue + 1, i => colorFilter.ProcessColorInCurve(i, curvef, curve));

            indColorFilters.Clear();
            return ApplyCurve(map, curve);
        }

        private static float[][] ConvertToCurveF(
            IEnumerable<IndependentComponentColorToColorFilter<float, float>> indColorFilters, int maxIndex)
        {
            var curvef = new[] { new float[maxIndex + 1], new float[maxIndex + 1], new float[maxIndex + 1] };

            Parallel.For(0, maxIndex + 1, i =>
            {
                var fi = i / (float)maxIndex;
                curvef[0][i] = fi;
                curvef[1][i] = fi;
                curvef[2][i] = fi;
            });

            foreach (var f in indColorFilters)
            {
                var filter = f;
                Parallel.For(0, maxIndex + 1, i => filter.ProcessColorInCurve(i, curvef, curvef));
            }
            return curvef;
        }

        private static Vector3[] ConvertToCurveV(
            IEnumerable<IndependentComponentVectorToVectorFilter> indColorFilters, int maxIndex)
        {
            var curvef = new Vector3[maxIndex + 1];

            Parallel.For(0, maxIndex + 1, i =>
            {
                var fi = i / (float)maxIndex;
                curvef[i] = new Vector3(fi);
            });

            foreach (var f in indColorFilters)
            {
                var filter = f;
                Parallel.For(0, maxIndex + 1, i => filter.ProcessColorInCurve(i, curvef, curvef));
            }
            return curvef;
        }

        private static ushort[][] ConvertToUshortCurve(float[][] input)
        {
            var maxIndex = input[0].Length - 1;
            var result = new[] { new ushort[maxIndex + 1], new ushort[maxIndex + 1], new ushort[maxIndex + 1] };

            Parallel.For(0, maxIndex + 1, i =>
            {
                result[0][i] = (ushort)(maxIndex * Math.Max(0, Math.Min(1f, input[0][i])));
                result[1][i] = (ushort)(maxIndex * Math.Max(0, Math.Min(1f, input[1][i])));
                result[2][i] = (ushort)(maxIndex * Math.Max(0, Math.Min(1f, input[2][i])));
            });
            return result;
        }


        private static ColorMapUshort ApplyIndependentColorToUshortFilters(ColorMapUshort map,
            ICollection<IndependentComponentColorToColorFilter<float, float>> indColorFilters)
        {
            if (indColorFilters.Count == 0) return map;

            var maxValue = map.MaxValue;
            var curvef = ConvertToCurveF(indColorFilters, maxValue);
            var curve = ConvertToUshortCurve(curvef);

            return ApplyCurve(map, curve);
        }

        private static ColorMapFloat ApplyIndependentColorToFloatFilters(ColorMapUshort map,
            ICollection<IndependentComponentColorToColorFilter<float, float>> indColorFilters)
        {
            if (indColorFilters.Count == 0) return map.ConvertToFloat();

            var maxValue = map.MaxValue;
            var curvef = ConvertToCurveF(indColorFilters, maxValue);

            return ApplyCurve(map, curvef);
        }

        private static VectorMap ApplyIndependentColorToVectorFilters(ColorMapUshort map,
            ICollection<IndependentComponentVectorToVectorFilter> indColorFilters)
        {
            if (indColorFilters.Count == 0) return map.ConvertToVector();

            var maxValue = map.MaxValue;
            var curvef = ConvertToCurveV(indColorFilters, maxValue);

            return ApplyCurve(map, curvef);
        }

        private static void ApplyIndependentColorFiltersInplace(ColorMapUshort map,
            ICollection<IndependentComponentColorToColorFilter<float, float>> indColorFilters)
        {
            if (indColorFilters.Count == 0) return;

            var maxValue = map.MaxValue;
            var curvef = ConvertToCurveF(indColorFilters, maxValue);
            var curve = ConvertToUshortCurve(curvef);

            ApplyCurveInplace(map, curve);
        }

        private static ColorMapUshort ApplyCurve(ColorMapUshort map, ushort[][] curve)
        {
            var result = new ColorMapUshort(map.Width, map.Height, map.MaxBits);
            Parallel.For(0, result.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[0][input.R], curve[1][input.G], curve[2][input.B]);
                    input.MoveNext();
                }
            });
            return result;
        }

        private static VectorMap ApplyCurve(ColorMapUshort map, Vector3[] curve)
        {
            var result = new VectorMap(map.Width, map.Height);
            Parallel.For(0, result.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[input.R].X, curve[input.G].Y, curve[input.B].Z);
                    input.MoveNext();
                }
            });
            return result;
        }

        private static ColorMapFloat ApplyCurve(ColorMap<ushort> map, float[][] curve)
        {
            var result = new ColorMapFloat(map.Width, map.Height);
            Parallel.For(0, result.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[0][input.R], curve[1][input.G], curve[2][input.B]);
                    input.MoveNext();
                }
            });
            return result;
        }

        private static void ApplyCurveInplace(ColorMap<ushort> map, ushort[][] curve)
        {
            Parallel.For(0, map.Height, y =>
            {
                var pix = map.GetRow(y);
                for (var x = 0; x < map.Width; x++)
                {
                    pix.SetAndMoveNext(curve[0][pix.R], curve[1][pix.G], curve[2][pix.B]);
                }
            });
        }

        private static RGB8Map ApplyCurve(ColorMap<ushort> map, byte[][] curve)
        {
            var result = new RGB8Map(map.Width, map.Height);

            Parallel.For(0, result.Height, y =>
            {
                var input = map.GetRow(y);
                var output = result.GetRow(y);
                for (var x = 0; x < result.Width; x++)
                {
                    output.SetAndMoveNext(curve[0][input.R], curve[1][input.G], curve[2][input.B]);
                    input.MoveNext();
                }
            });
            return result;
        }
    }
}