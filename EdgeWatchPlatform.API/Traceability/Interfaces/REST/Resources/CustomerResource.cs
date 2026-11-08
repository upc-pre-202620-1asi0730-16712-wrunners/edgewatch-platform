namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Representation of a customer.</summary>
public record CustomerResource(int Id, int SupplierOrganizationId, int? LinkedAssetOwnerOrganizationId, string LegalName, string Ruc, string MineSite);
