using Microsoft.Extensions.Configuration;
using Mindee;
using Mindee.Input;

namespace CarInsuranceBot.Infrastructure.Services.Mindee;

public class MindeeOcrService(IConfiguration config)
{
    private readonly MindeeClientV2 _mindeeClient = new(config["Mindee:ApiKey"]);
    private readonly string? _passwordModelId = config["Mindee:PasswordModelId"];
    private readonly string? _vehicleCertificateModelId = config["Mindee:VehicleCertificateModelId"];

    public async Task<Dictionary<string, string>> OcrPasswordAsync(string filePath)
    {
        var inferenceParams = new InferenceParameters(_passwordModelId, rag: false);
        var inputSource = new LocalInputSource(filePath);
        var response = await _mindeeClient.EnqueueAndGetInferenceAsync(inputSource, inferenceParams);

        var result = new Dictionary<string, string>();
        foreach (var kv in response.Inference.Result.Fields)
        {
            object? rawValue = kv.Value.SimpleField?.Value;
            string stringValue = rawValue?.ToString() ?? "";
            result[kv.Key] = stringValue;
        }
        return result;
    }

    public async Task<Dictionary<string, string>> OcrVehicleCertificateAsync(string filePath)
    {
        var inferenceParams = new InferenceParameters(_vehicleCertificateModelId, rag: false);
        var inputSource = new LocalInputSource(filePath);
        var response = await _mindeeClient.EnqueueAndGetInferenceAsync(inputSource, inferenceParams);

        var result = new Dictionary<string, string>();
        foreach (var kv in response.Inference.Result.Fields)
        {
            object? rawValue = kv.Value.SimpleField?.Value;
            string stringValue = rawValue?.ToString() ?? "";
            result[kv.Key] = stringValue;
        }
        return result;
    }
}