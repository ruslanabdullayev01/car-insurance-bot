using CarInsuranceBot.Application.IUnitOfWork;
using CarInsuranceBot.Infrastructure.Data;

namespace CarInsuranceBot.Infrastructure.UnitOfWork;
public sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    private readonly AppDbContext _db = db;
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _db.SaveChangesAsync(cancellationToken);
}
