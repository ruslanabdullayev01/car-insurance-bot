using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.MediatR.Base;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Application.MediatR.Queries.User
{
    public sealed record GetUsersQuery() : IQuery<IEnumerable<GetUsersResponse>>;

    public sealed record GetUsersResponse(string UserId,
       long TelegramUserId, string? FullName, string? State);


    public class GetAllUserHandler(IUserRepository userRepository) : IQueryHandler<GetUsersQuery, IEnumerable<GetUsersResponse>>
    {
        public async Task<Result<IEnumerable<GetUsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var response = await userRepository.FindAll(false)
               .OrderByDescending(u => u.CreatedDate)
               .Select(user => new GetUsersResponse(user.Id,
                  user.TelegramUserId,
                  user.FullName,
                  user.State.ToString()))
               .ToListAsync(cancellationToken: cancellationToken);

            return Result<IEnumerable<GetUsersResponse>>.Success(200, response);
        }
    }
}
