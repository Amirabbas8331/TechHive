

using TechHive.Domain.Abstraction;
using TechHive.Domain.Primitives;

namespace TechHive.Domain.ValueObjects;

public record ProductId(long Value) : EntityId<long>(Value)
{
    public static ProductId New(IIdGenerator<long> generator) => new ProductId(generator.NewId());
}

