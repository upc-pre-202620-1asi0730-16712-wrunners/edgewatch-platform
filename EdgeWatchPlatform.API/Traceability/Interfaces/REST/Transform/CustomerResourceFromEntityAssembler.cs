using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class CustomerResourceFromEntityAssembler
{
    public static CustomerResource ToResourceFromEntity(Customer entity) =>
        new(entity.Id, entity.SupplierOrganizationId, entity.LinkedAssetOwnerOrganizationId, entity.LegalName, entity.Ruc.Value, entity.MineSite);
}
