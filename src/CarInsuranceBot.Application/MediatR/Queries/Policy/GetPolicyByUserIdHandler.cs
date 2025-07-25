using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.Policy
{
    public sealed record GetPolicyByUserIdQuery(string UserId) : IQuery<GetPolicyByUserIdResponse>;

    public sealed record GetPolicyByUserIdResponse(
        string Id,
        string PolicyNumber,
        DateTime IssuedAt,
        DateTime ExpiryDate,
        string FilePath
    );

    public class GetPolicyByUserIdHandler(IPolicyRepository repository, IHttpContextAccessor httpContextAccessor)
        : IQueryHandler<GetPolicyByUserIdQuery, GetPolicyByUserIdResponse>
    {
        public async Task<Result<GetPolicyByUserIdResponse>> Handle(GetPolicyByUserIdQuery request, CancellationToken cancellationToken)
        {
            var baseUrl = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}/";
            var response = await repository
                .FindByCondition(x => x.UserId == request.UserId, false)
                .Select(x => new GetPolicyByUserIdResponse(x.Id,
                                                          x.PolicyNumber,
                                                          x.IssuedAt,
                                                          x.ExpiryDate,
                                                          baseUrl + x.FilePath))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if(response == null)
            {
                return Result<GetPolicyByUserIdResponse>.Fail(404, "Not found");
            }

            return Result<GetPolicyByUserIdResponse>.Success(200, response);
        }
    }
}