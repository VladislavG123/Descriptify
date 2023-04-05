using System.Linq.Expressions;
using Descriptify.Dal.Entities;
using Descriptify.Dal.Providers.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Descriptify.Dal.Providers.EntityFramework;

public abstract class BaseEfProvider<TEntity> : ICrudProvider<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;
    
    protected BaseEfProvider(ApplicationContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> GetById(Guid id)
        => await GetByIdOrDefault(id)
           ?? throw new ArgumentException($"No {typeof(TEntity).Name} with id \"{id}\"");

    public async Task<TEntity?> GetByIdOrDefault(Guid id)
        => await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
        => await GetByPredicateOrDefault(predicate)
           ?? throw new ArgumentException($"No such {typeof(TEntity).Name}");

    public async Task<TEntity?> GetByPredicateOrDefault(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate); 

    public async Task<IList<TEntity>> GetAll(int skip = 0, int take = Int32.MaxValue)
        => await _dbSet.Skip(skip).Take(take).ToListAsync();

    public async Task<IList<TEntity>> GetAll(
        Expression<Func<TEntity, bool>> predicate, int skip = 0, int take = Int32.MaxValue)
        => await _dbSet.Where(predicate).Skip(skip).Take(take).ToListAsync();

    public async Task Create(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task CreateMany(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task Update(TEntity entity)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        _dbSet.Remove(await GetById(id));
        await _context.SaveChangesAsync();
    }
}