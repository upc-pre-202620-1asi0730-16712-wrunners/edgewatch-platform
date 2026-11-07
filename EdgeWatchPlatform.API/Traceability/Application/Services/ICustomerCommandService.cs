using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface ICustomerCommandService
{
    Task<Result<Customer, RegisterCustomerError>> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken = default);
    Task<Result<Customer, UpdateCustomerError>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default);
}
