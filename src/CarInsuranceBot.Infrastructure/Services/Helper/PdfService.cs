using CarInsuranceBot.Application.IServices.Helper;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace CarInsuranceBot.Infrastructure.Services.Helper
{
    public class PdfService : IPdfService
    {
        public string GeneratePolicyPdf(Dictionary<string, string> fields, string fullName)
        {
            var filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
            using (var doc = new PdfDocument())
            {
                var page = doc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);

                int y = 40;
                gfx.DrawString($"Insurance Policy for {fullName}", font, XBrushes.Black, 20, y);
                y += 30;
                foreach (var field in fields)
                {
                    gfx.DrawString($"{field.Key}: {field.Value}", font, XBrushes.Black, 20, y);
                    y += 20;
                }
                doc.Save(filename);
            }
            return filename;
        }
    }
}
