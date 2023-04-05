using Descriptify.Dal.Entities;

namespace Descriptify.Dal.Providers.Abstract;

public interface ICrudProvider<TEntity> : IReadOnlyProvider<TEntity> where TEntity : BaseEntity
{
    Task Create(TEntity entity);
    Task CreateMany(IEnumerable<TEntity> entities);
    Task Update(TEntity entity);
    Task Delete(Guid id);
}