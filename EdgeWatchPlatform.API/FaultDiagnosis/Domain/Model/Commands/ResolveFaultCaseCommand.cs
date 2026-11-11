namespace EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;

public record ResolveFaultCaseCommand(int FaultCaseId, int UserId, string? ResolutionNotes);
