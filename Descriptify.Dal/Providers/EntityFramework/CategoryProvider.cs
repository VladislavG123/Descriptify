using Descriptify.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Descriptify.Dal.Providers.EntityFramework;

public class CategoryProvider : BaseEfProvider<CategoryEntity>, ICategoryProvider
{
    private readonly ApplicationContext _context;

    public CategoryProvider(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryEntity>> GetChildren(Guid categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CategoryEntity>> GetParents(Guid categoryId)
    {
        var currentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
        if (currentCategory is null)
        {
            throw new ArgumentException("No category with the given id");
        }
        
        var result = new List<CategoryEntity>();
        while (currentCategory.ParentCategory is not null)
        {
            currentCategory = currentCategory.ParentCategory;
            result.Add(currentCategory);
        }
        
        return result;
    }
}