using System.Linq.Expressions;
using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Infrastructure.Data;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    public GenericRepository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<T>();
    }

    public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? _dbSet.AsNoTracking().AsQueryable() : _dbSet;
    public async Task<T?> FindByIdAsync(string Id, bool trackChanges = true) => trackChanges ? await _dbSet.FindAsync(Id) :
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges) => !trackChanges ?
        _dbSet.Where(condition).AsNoTracking() :
        _dbSet.Where(condition);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression) => await _dbSet.AnyAsync(expression);
    public void Create(T entity) => _dbSet.Add(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public void DeleteAddRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}
