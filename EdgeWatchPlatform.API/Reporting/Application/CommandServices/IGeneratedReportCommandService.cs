using EdgeWatchPlatform.API.Reporting.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.Reporting.Application.CommandServices;

public interface IGeneratedReportCommandService
{
    Task<Result<GeneratedReport>> Handle(RegisterGeneratedReportCommand command, CancellationToken cancellationToken = default);
}
