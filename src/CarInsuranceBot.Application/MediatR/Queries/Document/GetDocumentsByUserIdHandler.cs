using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.Document
{
    public sealed record GetDocumentsResponse(
        string DocumentId,
        string FileName,
        string FilePath,
        DocumentType Type,
        string UserId
    );

    public sealed record GetDocumentsByUserIdQuery(string UserId) : IQuery<IEnumerable<GetDocumentsResponse>>;


    public class GetDocumentsByUserIdHandler(IDocumentRepository documentRepository) : IQueryHandler<GetDocumentsByUserIdQuery, IEnumerable<GetDocumentsResponse>>
    {
        public async Task<Result<IEnumerable<GetDocumentsResponse>>> Handle(GetDocumentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = await documentRepository.FindAll(false)
                .Where(d => d.UserId == request.UserId)
                .OrderByDescending(d => d.CreatedDate)
                .Select(doc => new GetDocumentsResponse(
                    doc.Id,
                    doc.FileName,
                    doc.FilePath,
                    doc.Type,
                    doc.UserId
                ))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<GetDocumentsResponse>>.Success(200, response);
        }
    }
}
