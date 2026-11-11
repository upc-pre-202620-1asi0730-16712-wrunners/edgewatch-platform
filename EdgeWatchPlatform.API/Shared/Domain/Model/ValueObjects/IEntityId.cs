namespace EdgeWatchPlatform.API.Shared.Domain.Model.ValueObjects;

/// <summary>Strongly-typed entity identifier contract.</summary>
/// <typeparam name="T">The underlying primitive type.</typeparam>
public interface IEntityId<T> where T : struct
{
    /// <summary>Underlying primitive value.</summary>
    T Value { get; }
}
