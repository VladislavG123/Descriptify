using Descriptify.Dal.Entities;
using Descriptify.Dal.Providers.Abstract;

namespace Descriptify.Dal.Providers;

public interface ICategoryProvider : ICrudProvider<CategoryEntity>
{
    Task<IEnumerable<CategoryEntity>> GetChildren(Guid categoryId);
    Task<IEnumerable<CategoryEntity>> GetParents(Guid categoryId);
}