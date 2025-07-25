namespace CarInsuranceBot.Application.IServices
{
    public interface IPolicyService
    {
        Task CreatePolicyAsync(string filePath, string userId);
    }
}
