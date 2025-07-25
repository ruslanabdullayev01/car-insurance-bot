using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.GetExtractedField
{
    public sealed record GetExtractedFieldResponse(
        string Id,
        string Key,
        string Value,
        string UserId
    );
    public sealed record GetExtractedFieldsByUserIdQuery(string UserId) : IQuery<IEnumerable<GetExtractedFieldResponse>>;

    public class GetExtractedFieldsByUserIdHandler(IExtractedFieldRepository repository)
        : IQueryHandler<GetExtractedFieldsByUserIdQuery, IEnumerable<GetExtractedFieldResponse>>
    {
        public async Task<Result<IEnumerable<GetExtractedFieldResponse>>> Handle(GetExtractedFieldsByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var response = await repository.FindAll(false)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new GetExtractedFieldResponse(
                    x.Id,
                    x.Key,
                    x.Value,
                    x.UserId
                ))
                .ToListAsync(cancellationToken);
            return Result<IEnumerable<GetExtractedFieldResponse>>.Success(200, response);
        }
    }
}
