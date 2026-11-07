using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record RegisterCustomerCommand(int SupplierOrganizationId, int? LinkedAssetOwnerOrganizationId, string LegalName, Ruc Ruc, string MineSite);
