namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record UpdateCustomerCommand(int CustomerId, int? LinkedAssetOwnerOrganizationId, string LegalName, string MineSite);
