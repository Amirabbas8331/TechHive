
namespace TechHive.Domain.Base;

public abstract class Entity<TId> : IEntity<TId>
    where TId : notnull
{
    public TId Id { get; }
    protected Entity(TId id)=>Id = id;
}
