using CarInsuranceBot.Application.DTOs.OpenAI;

namespace CarInsuranceBot.Application.IServices.Helper
{
    public interface IOpenAIService
    {
        Task<string> GetResponseAsync(RequestDto dto);
    }
}
