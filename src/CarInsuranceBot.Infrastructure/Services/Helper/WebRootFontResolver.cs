using PdfSharp.Fonts;

namespace CarInsuranceBot.Infrastructure.Services.Helper
{
    public class WebRootFontResolver(string webRootPath) : IFontResolver
    {
        private readonly string _fontPath = Path.Combine(webRootPath, "Fonts", "Roboto-Regular.ttf");

        public byte[] GetFont(string faceName)
        {
            return File.ReadAllBytes(_fontPath);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("Roboto");
        }
    }
}
