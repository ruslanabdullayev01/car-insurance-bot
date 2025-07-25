namespace CarInsuranceBot.Application.IServices
{
    public interface IExtractedFieldService
    {
        Task SaveExtractedFieldAsync(string userId, string key, string value);
        Task<Dictionary<string, string>> GetAllNonEmptyByUserIdAsync(string userId);
    }
}
