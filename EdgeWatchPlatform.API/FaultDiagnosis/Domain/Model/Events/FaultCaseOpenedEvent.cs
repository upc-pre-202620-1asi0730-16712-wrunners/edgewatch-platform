using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Events;

public record FaultCaseOpenedEvent(int OrganizationId, int SpraySessionId, string Parameter, string Band, string ProbableCause) : IEvent;
