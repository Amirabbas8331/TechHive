namespace TechHive.Infrastructure.Repositories;

using global::TechHive.Domain.Abstraction;


public class SequentialLongIdGenerator : IIdGenerator<long>
{
    private long _current = 0;
    public long NewId()
    {
        return Interlocked.Increment(ref _current);
    }
}