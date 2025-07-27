namespace CarInsuranceBot.Application.IServices.Helpers
{
    public interface IPdfService
    {
        string GeneratePolicyPdf(Dictionary<string, string> fields, string fullName);
    }
}