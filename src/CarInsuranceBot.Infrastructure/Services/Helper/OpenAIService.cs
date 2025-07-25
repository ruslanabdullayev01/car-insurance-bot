using CarInsuranceBot.Application.DTOs.OpenAI;
using CarInsuranceBot.Application.IServices.Helper;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace CarInsuranceBot.Infrastructure.Services.Helper
{
    public sealed class OpenAIService(HttpClient httpClient, IConfiguration configuration) : IOpenAIService
    {
        private readonly string _openAiApiUrl = configuration["OpenAi:ApiUrl"] ?? throw new Exception(nameof(_openAiApiUrl));
        private readonly string _openAiApiKey = configuration["OpenAi:ApiKey"] ?? throw new Exception(nameof(_openAiApiKey));

        public async Task<string> GetResponseAsync(RequestDto dto)
        {
            var prompt = BuildPromptFromMember(dto.UserMessage);

            var requestPayload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000
            };

            var jsonPayload = JsonSerializer.Serialize(requestPayload);

            var request = new HttpRequestMessage(HttpMethod.Post, _openAiApiUrl);
            request.Headers.Add("Authorization", $"Bearer {_openAiApiKey}");
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseContent);
            var chatGptResponse = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            var sanitizedResponse = chatGptResponse?.Replace("\n", string.Empty) ?? string.Empty;

            return sanitizedResponse.Trim();

        }


        private static string BuildPromptFromMember(string userMessage)
        {
            return $"You are an assistant for a Car Insurance Bot. " +
                $"Your job is to answer user questions about car insurance in a simple and polite way. " +
                $"If the question is not related to car insurance, respond briefly but friendly. Here is the user’s question: {userMessage}";
        }
    }
}
