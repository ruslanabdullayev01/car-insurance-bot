using PdfSharp.Fonts;

namespace CarInsuranceBot.Application.IServices.Helpers
{
    public interface IFontResolver 
    {
        byte[] GetFont(string faceName);
        FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic);
    }
}
