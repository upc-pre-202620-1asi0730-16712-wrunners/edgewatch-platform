using System.Text.Json.Serialization;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.Events;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;

/// <summary>A user of an organization, identified by email, with a set of roles.</summary>
public partial class User : AggregateRoot
{
    private readonly List<UserRole> _roles = [];

    protected User()
    {
        FullName = string.Empty;
        Email = null!;
        PasswordHash = string.Empty;
    }

    public User(string fullName, EmailAddress email, string passwordHash, int organizationId) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        FullName = fullName.Trim();
        Email = email;
        PasswordHash = passwordHash;
        OrganizationId = organizationId;
        Status = UserStatus.Active;
        AddDomainEvent(new UserRegisteredEvent(Email.Value, FullName, OrganizationId));
    }

    public int Id { get; private set; }
    public string FullName { get; private set; }
    public EmailAddress Email { get; private set; }
    [JsonIgnore] public string PasswordHash { get; private set; }
    public int OrganizationId { get; private set; }
    public UserStatus Status { get; private set; }
    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

    public bool HasRole(Role role) => _roles.Any(r => r.RoleId == role.Id);

    public bool HasRole(int roleId) => _roles.Any(r => r.RoleId == roleId);

    /// <summary>Adds a role when the user does not have it yet.</summary>
    public User AddRole(Role role)
    {
        if (!HasRole(role)) _roles.Add(new UserRole(role.Id));
        return this;
    }

    /// <summary>Replaces every role of the user.</summary>
    public User ReplaceRoles(IEnumerable<Role> roles)
    {
        _roles.Clear();
        foreach (var role in roles.DistinctBy(r => r.Id)) _roles.Add(new UserRole(role.Id));
        return this;
    }

    public User UpdatePasswordHash(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        PasswordHash = passwordHash;
        return this;
    }
}
