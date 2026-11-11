namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

public record AcknowledgeFaultCaseCommand(int FaultCaseId, int UserId);
