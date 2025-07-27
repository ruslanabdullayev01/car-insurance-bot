using CarInsuranceBot.Application.DTOs.OpenAI;

namespace CarInsuranceBot.Application.IServices.OpenAI
{
    public interface IOpenAIService
    {
        Task<string> GetResponseAsync(RequestDto dto);
    }
}
