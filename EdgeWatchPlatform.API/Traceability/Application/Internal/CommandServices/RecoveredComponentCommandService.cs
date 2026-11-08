using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.CommandServices;

public class RecoveredComponentCommandService(
    IRecoveredComponentRepository componentRepository,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ILogger<RecoveredComponentCommandService> logger) : IRecoveredComponentCommandService
{
    public async Task<Result<RecoveredComponent, RegisterComponentError>> Handle(RegisterComponentCommand command, CancellationToken cancellationToken = default)
    {
        if (await customerRepository.FindByIdAsync(command.CustomerId, cancellationToken) is null)
        {
            logger.LogWarning("Customer {CustomerId} not found registering component {SerialNumber}", command.CustomerId, command.SerialNumber);
            return new Result<RecoveredComponent, RegisterComponentError>.Failure(RegisterComponentError.CustomerNotFound);
        }

        if (await componentRepository.ExistsByCustomerIdAndSerialNumberAsync(command.CustomerId, command.SerialNumber.Trim(), cancellationToken))
        {
            logger.LogWarning("Duplicate component serial number {SerialNumber} for customer {CustomerId}", command.SerialNumber, command.CustomerId);
            return new Result<RecoveredComponent, RegisterComponentError>.Failure(RegisterComponentError.DuplicateSerialNumber);
        }

        try
        {
            var component = new RecoveredComponent(command);
            await componentRepository.AddAsync(component, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<RecoveredComponent, RegisterComponentError>.Success(component);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error registering component {SerialNumber} for customer {CustomerId}", command.SerialNumber, command.CustomerId);
            return new Result<RecoveredComponent, RegisterComponentError>.Failure(RegisterComponentError.UnexpectedError);
        }
    }

    public async Task<Result<RecoveredComponent, UpdateComponentError>> Handle(UpdateComponentCommand command, CancellationToken cancellationToken = default)
    {
        var component = await componentRepository.FindByIdAsync(command.ComponentId, cancellationToken);
        if (component is null) return new Result<RecoveredComponent, UpdateComponentError>.Failure(UpdateComponentError.ComponentNotFound);

        try
        {
            component.Update(command);
            componentRepository.Update(component);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<RecoveredComponent, UpdateComponentError>.Success(component);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating component {ComponentId}", command.ComponentId);
            return new Result<RecoveredComponent, UpdateComponentError>.Failure(UpdateComponentError.UnexpectedError);
        }
    }
}
