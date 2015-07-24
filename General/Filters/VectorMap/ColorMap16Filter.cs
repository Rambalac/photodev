using com.azi.Image;

namespace com.azi.Filters.ColorMap16
{
    internal interface IVectorMapFilter : IFilter
    {
        VectorImageFile Process(VectorImageFile image);
    }
}