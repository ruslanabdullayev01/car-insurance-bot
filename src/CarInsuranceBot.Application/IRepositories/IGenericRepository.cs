using System.Linq.Expressions;

namespace CarInsuranceBot.Application.IRepositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteAddRange(IEnumerable<T> entities);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task<T?> FindByIdAsync(string Id, bool trackChanges = true);
}
