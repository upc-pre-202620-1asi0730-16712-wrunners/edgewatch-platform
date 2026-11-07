using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class RegisterCustomerCommandFromResourceAssembler
{
    public static RegisterCustomerCommand ToCommandFromResource(RegisterCustomerResource resource) =>
        new(resource.SupplierOrganizationId, resource.LinkedAssetOwnerOrganizationId, resource.LegalName, new Ruc(resource.Ruc), resource.MineSite);
}
