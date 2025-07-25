using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.Error
{
    public sealed record GetErrorsQuery() : IQuery<IEnumerable<GetErrorResponse>>;

    public sealed record GetErrorResponse(
        string Id,
        string Message,
        string? StackTrace,
        DateTime OccurredAt,
        string? UserId,
        string? Context
    );

    public class GetErrorsHandler(IErrorRepository repository) : IQueryHandler<GetErrorsQuery, IEnumerable<GetErrorResponse>>
    {
        public async Task<Result<IEnumerable<GetErrorResponse>>> Handle(GetErrorsQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.FindAll(false)
                .OrderByDescending(x => x.OccurredAt)
                .Select(x => new GetErrorResponse(
                    x.Id,
                    x.Message,
                    x.StackTrace,
                    x.OccurredAt,
                    x.UserId,
                    x.Context
                ))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<GetErrorResponse>>.Success(200, response);
        }
    }
}
