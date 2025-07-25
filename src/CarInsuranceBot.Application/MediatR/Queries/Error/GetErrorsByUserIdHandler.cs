using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.Error
{
    public sealed record GetErrorsByUserIdQuery(string UserId) : IQuery<IEnumerable<GetErrorByUserIdResponse>>;

    public sealed record GetErrorByUserIdResponse(
        string Id,
        string Message,
        string? StackTrace,
        DateTime OccurredAt,
        string? UserId,
        string? Context
    );

    public class GetErrorsByUserIdHandler(IErrorRepository repository)
        : IQueryHandler<GetErrorsByUserIdQuery, IEnumerable<GetErrorByUserIdResponse>>
    {
        public async Task<Result<IEnumerable<GetErrorByUserIdResponse>>> Handle(GetErrorsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.FindAll(false)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.OccurredAt)
                .Select(x => new GetErrorByUserIdResponse(
                    x.Id,
                    x.Message,
                    x.StackTrace,
                    x.OccurredAt,
                    x.UserId,
                    x.Context
                ))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<GetErrorByUserIdResponse>>.Success(200, response);
        }
    }
}
