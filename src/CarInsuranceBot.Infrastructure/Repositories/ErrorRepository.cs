using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities;

namespace CarInsuranceBot.Infrastructure.Repositories
{
    public class ErrorRepository(AppDbContext context) : GenericRepository<Error>(context), IErrorRepository
    {
    }
}
