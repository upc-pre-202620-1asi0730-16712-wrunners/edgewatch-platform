using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Entities;

/// <summary>A role that can be assigned to users. Seeded on startup from <see cref="Roles"/>.</summary>
public class Role
{
    protected Role()
    {
    }

    public Role(Roles name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public Roles Name { get; private set; }

    /// <summary>Role name as exposed by the API, e.g. ROLE_ORG_ADMIN.</summary>
    public string StringName => Name.ToUpperSnakeCase();

    public static Role ToRoleFromName(string name) => new(name.ToEnum<Roles>());

    /// <summary>Database id of a role: roles are seeded in declaration order, so the id is the ordinal plus one.</summary>
    public static int ToIdFromName(Roles name) => (int)name + 1;

    /// <summary>Default role of the first user of an organization.</summary>
    public static Role GetDefaultRole() => new(Roles.RoleOrgAdmin);
}
