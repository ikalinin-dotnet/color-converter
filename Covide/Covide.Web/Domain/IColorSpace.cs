namespace Covide.Web.Domain
{
    public interface IColorSpace<TRepresentation>
    {
        TRepresentation FromRgb(RgbColor rgb);
        RgbColor ToRgb(TRepresentation value);
    }
}
