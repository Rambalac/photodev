using com.azi.image;

namespace com.azi.Debayer
{
    public interface IDebayer
    {
        ColorMap<ushort> Debayer(RawImageFile file);
    }

    public interface IBGGRDebayer: IDebayer
    {
    }
}