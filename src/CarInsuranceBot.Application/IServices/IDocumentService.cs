using CarInsuranceBot.Application.DTOs.Document;
using Microsoft.AspNetCore.Hosting;

namespace CarInsuranceBot.Application.IServices
{
    public interface IDocumentService
    {
        Task<SavedDocumentResult> SaveDocumentAsync(SaveDocumentRequest request, IWebHostEnvironment env);
    }
}
