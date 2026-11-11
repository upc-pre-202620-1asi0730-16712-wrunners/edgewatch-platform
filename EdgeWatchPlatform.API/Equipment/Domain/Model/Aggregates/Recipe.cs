using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;

/// <summary>A spray recipe of an HVOF system: powder, applicabilities and threshold bands per parameter.</summary>
public partial class Recipe : AggregateRoot
{
    private readonly List<RecipeApplicability> _applicabilities = [];
    private readonly List<RecipeParameter> _parameters = [];

    protected Recipe()
    {
        Name = string.Empty;
        PowderSpecification = string.Empty;
    }

    public Recipe(CreateRecipeCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.RecipeNumber, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);
        if (command.Parameters.Count == 0) throw new ArgumentException("A recipe needs at least one parameter.", nameof(command));
        if (command.Parameters.Select(p => p.Parameter.Value).Distinct().Count() != command.Parameters.Count)
            throw new ArgumentException("Recipe parameters must be unique.", nameof(command));
        HvofSystemId = command.HvofSystemId;
        RecipeNumber = command.RecipeNumber;
        Name = command.Name.Trim();
        PowderSpecification = command.PowderSpecification.Trim();
        Status = RecipeStatus.Draft;
        foreach (var applicability in command.Applicabilities) _applicabilities.Add(applicability);
        foreach (var item in command.Parameters) _parameters.Add(new RecipeParameter(item.Parameter, item.Bands, item.UnitSymbol, item.UnitCategory));
    }

    public int Id { get; private set; }
    public int HvofSystemId { get; private set; }
    public int RecipeNumber { get; private set; }
    public string Name { get; private set; }
    public string PowderSpecification { get; private set; }
    public RecipeStatus Status { get; private set; }
    public IReadOnlyCollection<RecipeApplicability> Applicabilities => _applicabilities.AsReadOnly();
    public IReadOnlyCollection<RecipeParameter> Parameters => _parameters.AsReadOnly();

    public bool IsActive => Status == RecipeStatus.Active;

    public RecipeParameter? ParameterFor(string parameter) => _parameters.FirstOrDefault(p => p.Parameter == parameter);

    /// <summary>DRAFT → ACTIVE. Returns false for any other status.</summary>
    public bool TryPublish()
    {
        if (Status != RecipeStatus.Draft) return false;
        Status = RecipeStatus.Active;
        AddDomainEvent(new RecipePublishedEvent(Id, HvofSystemId, RecipeNumber, Name));
        return true;
    }

    /// <summary>ACTIVE → RETIRED. Returns false for any other status.</summary>
    public bool TryRetire()
    {
        if (Status != RecipeStatus.Active) return false;
        Status = RecipeStatus.Retired;
        return true;
    }
}
