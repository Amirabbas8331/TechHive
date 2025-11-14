

using TechHive.Domain.Primitives;

namespace TechHive.Domain.ValueObjects;

public record ProductId(long Value) : EntityId<long>(Value)
{
    //? Guid Built in
    // public static ProductId New() => new ProductId(Guid.NewGuid());

    //public static ProductId New(IIdGenerator<long> generator) => new ProductId(generator.NewId());
}

