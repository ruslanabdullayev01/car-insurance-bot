using CarInsuranceBot.Application.DTOs.Document;
using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using Domain.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace CarInsuranceBot.Infrastructure.Services;

public class DocumentService(IDocumentRepository documentRepository,
                             IUnitOfWork unitOfWork,
                             IDateTimeProvider dateTime) : IDocumentService
{
    private readonly string[] allowedContentTypes = ["image/"];

    public async Task<SavedDocumentResult> SaveDocumentAsync(SaveDocumentRequest request, IWebHostEnvironment env)
    {
        if (!allowedContentTypes.Any(request.FilePath.ContentType.Contains))
            throw new Exception("The uploaded file must be an image.");

        if (request.FilePath.CheckFileLength())
            throw new Exception("The file size must not exceed 5 mb.");

        string savedPath = await request.FilePath.CreateFileAsync(env, "Images/Document");
        string absolutePath = Path.Combine(env.WebRootPath, savedPath);

        var document = new Domain.Entities.Document()
        {
            UserId = request.UserId,
            FilePath = savedPath,
            FileName = request.FileName,
            Type = request.DocumentType,
            CreatedDate = dateTime.UtcNow
        };

        documentRepository.Create(document);
        await unitOfWork.SaveChangesAsync();

        return new SavedDocumentResult(savedPath, absolutePath);
    }
}