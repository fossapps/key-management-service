using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Storage.Generators;

public class IdValueGenerator : ValueGenerator<string>
{
    public override bool GeneratesTemporaryValues { get; }

    public override string Next(EntityEntry entry)
    {
        var modelName = entry.Entity.GetType().Name.ToLower();
        var id = Ulid.NewUlid().ToString();
        return $"{modelName}_{id}";
    }
}
