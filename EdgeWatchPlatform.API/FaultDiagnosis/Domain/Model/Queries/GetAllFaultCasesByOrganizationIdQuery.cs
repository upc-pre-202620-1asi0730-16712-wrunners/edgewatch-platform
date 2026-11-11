using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;

/// <summary>Cases of an organization; spray session and status are optional filters.</summary>
public record GetAllFaultCasesByOrganizationIdQuery(int OrganizationId, int? SpraySessionId = null, FaultCaseStatus? Status = null);
