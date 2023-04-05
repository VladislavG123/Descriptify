namespace Descriptify.Dal.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}