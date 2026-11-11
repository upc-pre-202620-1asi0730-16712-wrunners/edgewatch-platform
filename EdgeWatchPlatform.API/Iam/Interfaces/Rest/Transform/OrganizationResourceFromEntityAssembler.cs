using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;

public static class OrganizationResourceFromEntityAssembler
{
    public static OrganizationResource ToResourceFromEntity(Organization organization) =>
        new(organization.Id, organization.Name, organization.Ruc.Value, organization.OrganizationType.ToUpperSnakeCase(),
            organization.Status.ToUpperSnakeCase(), organization.TimeZone);
}
