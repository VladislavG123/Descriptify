using System.Linq.Expressions;
using Descriptify.Dal.Entities;

namespace Descriptify.Dal.Providers.Abstract;

public interface IReadOnlyProvider<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetById(Guid id);
    Task<TEntity?> GetByIdOrDefault(Guid id);
    Task<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByPredicateOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<IList<TEntity>> GetAll(int skip = 0, int take = Int32.MaxValue);
    Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, int skip = 0, int take = Int32.MaxValue);
}