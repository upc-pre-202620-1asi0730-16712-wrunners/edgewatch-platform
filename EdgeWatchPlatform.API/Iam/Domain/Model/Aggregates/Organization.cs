using EdgeWatchPlatform.API.Iam.Domain.Model.Commands;
using EdgeWatchPlatform.API.Iam.Domain.Model.Events;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;

/// <summary>A tenant of the platform: a recuperation supplier or an asset owner.</summary>
public partial class Organization : AggregateRoot
{
    protected Organization()
    {
        Name = string.Empty;
        Ruc = null!;
        TimeZone = "America/Lima";
    }

    public Organization(SignUpCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.OrganizationName);
        Name = command.OrganizationName.Trim();
        Ruc = command.Ruc;
        OrganizationType = command.OrganizationType;
        TimeZone = string.IsNullOrWhiteSpace(command.TimeZone) ? "America/Lima" : command.TimeZone.Trim();
        Status = OrganizationStatus.Active;
        AddDomainEvent(new OrganizationRegisteredEvent(Name, Ruc.Value, OrganizationType.ToUpperSnakeCase()));
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public Ruc Ruc { get; private set; }
    public OrganizationType OrganizationType { get; private set; }
    public OrganizationStatus Status { get; private set; }
    public string TimeZone { get; private set; }

    public bool IsSupplier => OrganizationType == OrganizationType.RecuperationSupplier;
}
