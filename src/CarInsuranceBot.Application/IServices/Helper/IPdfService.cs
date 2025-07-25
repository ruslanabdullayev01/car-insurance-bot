namespace CarInsuranceBot.Application.IServices.Helper
{
    public interface IPdfService
    {
        string GeneratePolicyPdf(Dictionary<string, string> fields, string fullName);
    }
}