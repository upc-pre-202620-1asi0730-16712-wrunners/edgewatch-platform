namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;

public record StartSpraySessionCommand(int HvofSystemId, int RecuperationId, int OperatorId, int RecipeNumber, string TimeZone);
