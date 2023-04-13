namespace Descriptify.Dal.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; }
    public virtual CategoryEntity? ParentCategory { get; set; }
}