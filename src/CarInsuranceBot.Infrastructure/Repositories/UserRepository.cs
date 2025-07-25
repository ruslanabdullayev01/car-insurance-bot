using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Repositories
{
    public sealed class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}
