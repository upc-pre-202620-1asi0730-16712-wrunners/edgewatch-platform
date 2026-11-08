using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;

/// <summary>A mining customer served by a recuperation supplier organization.</summary>
public partial class Customer
{
    protected Customer()
    {
        LegalName = string.Empty;
        Ruc = null!;
        MineSite = string.Empty;
    }

    public Customer(RegisterCustomerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        SupplierOrganizationId = command.SupplierOrganizationId;
        LinkedAssetOwnerOrganizationId = command.LinkedAssetOwnerOrganizationId;
        LegalName = command.LegalName.Trim();
        Ruc = command.Ruc;
        MineSite = command.MineSite.Trim();
    }

    public int Id { get; private set; }
    public int SupplierOrganizationId { get; private set; }
    public int? LinkedAssetOwnerOrganizationId { get; private set; }
    public string LegalName { get; private set; }
    public Ruc Ruc { get; private set; }
    public string MineSite { get; private set; }

    /// <summary>Updates the editable data. The RUC is the fiscal identity and is never changed.</summary>
    public void Update(UpdateCustomerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        LegalName = command.LegalName.Trim();
        MineSite = command.MineSite.Trim();
        LinkedAssetOwnerOrganizationId = command.LinkedAssetOwnerOrganizationId;
    }
}
