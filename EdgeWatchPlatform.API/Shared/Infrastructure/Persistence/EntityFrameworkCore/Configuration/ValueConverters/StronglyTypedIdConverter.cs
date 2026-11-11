using EdgeWatchPlatform.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

/// <summary>Value converter between a strongly-typed identifier and its primitive column value.</summary>
public class StronglyTypedIdConverter<TId, TValue>(Func<TId, TValue> convertToProvider, Func<TValue, TId> convertFromProvider)
    : ValueConverter<TId, TValue>(id => convertToProvider(id), val => convertFromProvider(val))
    where TValue : struct
{
    public StronglyTypedIdConverter(Func<TValue, TId> factory)
        : this(id => ((IEntityId<TValue>)id!).Value, factory)
    {
    }
}
