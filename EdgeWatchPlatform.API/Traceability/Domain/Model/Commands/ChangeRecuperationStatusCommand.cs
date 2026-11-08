using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record ChangeRecuperationStatusCommand(int RecuperationId, RecuperationStatus Status);
