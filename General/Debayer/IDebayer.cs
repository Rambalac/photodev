using System;
using com.azi.Image;

namespace com.azi.Debayer
{
    public interface IDebayer<in T> where T : RawMap<ushort>
    {
        ColorMap<ushort> Debayer(T map);
    }

    public interface IBGGRDebayer : IDebayer<RawBGGRMap<ushort>>
    {
    }
}