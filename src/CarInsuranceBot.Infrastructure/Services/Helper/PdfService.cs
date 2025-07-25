using CarInsuranceBot.Application.IServices.Helper;
using Microsoft.AspNetCore.Hosting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace CarInsuranceBot.Infrastructure.Services.Helper
{
    public class PdfService(IWebHostEnvironment webHostEnvironment) : IPdfService
    {
        public string GeneratePolicyPdf(Dictionary<string, string> fields, string fullName)
        {
            var path = Path.Combine(webHostEnvironment.WebRootPath, "Policies");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filename = Path.Combine(path, $"{Guid.NewGuid()}.pdf");
            using (var doc = new PdfDocument())
            {
                var page = doc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                // Fonts
                var titleFont = new XFont("Roboto", 20, XFontStyleEx.Bold);
                var headerFont = new XFont("Roboto", 14, XFontStyleEx.Bold);
                var labelFont = new XFont("Roboto", 12, XFontStyleEx.Bold);
                var valueFont = new XFont("Roboto", 12);

                // Title
                gfx.DrawString("INSURANCE POLICY", titleFont, XBrushes.DarkBlue, new XRect(0, 40, page.Width, 50), XStringFormats.TopCenter);

                int y = 100;
                gfx.DrawString($"Policy Holder: {fullName}", headerFont, XBrushes.Black, 20, y);
                y += 30;

                // Draw Sections
                gfx.DrawRectangle(XPens.Black, 15, y, page.Width - 30, 25 * fields.Count + 20);

                int rowY = y + 10;
                foreach (var field in fields)
                {
                    gfx.DrawString(field.Key + ":", labelFont, XBrushes.Black, 25, rowY);
                    gfx.DrawString(field.Value, valueFont, XBrushes.Black, 200, rowY);
                    rowY += 25;
                }

                // Signature
                rowY += 40;
                gfx.DrawString("Signature: ___________________", labelFont, XBrushes.Black, 25, rowY);
                rowY += 30;
                gfx.DrawString("Company Seal", labelFont, XBrushes.Gray, 400, rowY);

                doc.Save(filename);
            }
            return filename;
        }
    }
}
