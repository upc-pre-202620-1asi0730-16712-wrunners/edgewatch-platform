namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

public record DeviationCountResource(int SpraySessionId, int Warning, int Shutdown, int Total);
