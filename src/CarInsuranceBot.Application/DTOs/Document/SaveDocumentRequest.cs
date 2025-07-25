using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CarInsuranceBot.Application.DTOs.Document
{
    public sealed record SaveDocumentRequest(string UserId, IFormFile FilePath, DocumentType DocumentType, string FileName);
}
