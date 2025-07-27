using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.Conversation
{
    public sealed record GetConversationsByUserIdQuery(string UserId, int Page, int PageSize) : IQuery<IEnumerable<GetConversationByUserIdResponse>>;

    public sealed record GetConversationByUserIdResponse(
        string Id,
        string Request,
        string Response,
        DateTime CreatedDate
    );

    public class GetConversationsByUserIdHandler(IConversationRepository repository)
        : IQueryHandler<GetConversationsByUserIdQuery, IEnumerable<GetConversationByUserIdResponse>>
    {
        public async Task<Result<IEnumerable<GetConversationByUserIdResponse>>> Handle(GetConversationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize is <= 0 or > 100 ? 10 : request.PageSize;

            var response = await repository.FindAll(false)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetConversationByUserIdResponse(
                    x.Id,
                    x.Request,
                    x.Response,
                    x.CreatedDate
                ))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<GetConversationByUserIdResponse>>.Success(200, response);
        }
    }
}
