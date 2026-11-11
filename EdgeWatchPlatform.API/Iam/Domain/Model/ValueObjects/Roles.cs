namespace EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;

/// <summary>Roles of the platform. The database id of each role is its ordinal plus one (ROLE_ORG_ADMIN = 1).</summary>
public enum Roles
{
    RoleOrgAdmin,
    RoleQualityEngineer,
    RoleMaintenanceSupervisor,
    RoleHvofOperator,
    RoleReliabilityEngineer,
    RoleOperationsSupervisor,
    RoleProcurementAnalyst
}
