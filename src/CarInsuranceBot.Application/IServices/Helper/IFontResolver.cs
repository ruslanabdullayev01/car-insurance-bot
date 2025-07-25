using PdfSharp.Fonts;

namespace CarInsuranceBot.Application.IServices.Helper
{
    public interface IFontResolver 
    {
        byte[] GetFont(string faceName);
        FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic);
    }
}
