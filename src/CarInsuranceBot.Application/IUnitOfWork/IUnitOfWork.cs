namespace CarInsuranceBot.Application.IUnitOfWork;
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
