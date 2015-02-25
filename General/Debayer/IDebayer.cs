using com.azi.image;

namespace com.azi.Debayer
{
    public interface IDebayer
    {
        ColorMap Debayer(RawImageFile file);
    }

    public interface IBGGRDebayer: IDebayer
    {
    }
}