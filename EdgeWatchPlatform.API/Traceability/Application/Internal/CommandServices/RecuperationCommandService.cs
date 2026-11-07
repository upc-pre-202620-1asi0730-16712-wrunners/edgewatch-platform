using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.CommandServices;

public class RecuperationCommandService(
    IRecuperationRepository recuperationRepository,
    IRecoveredComponentRepository componentRepository,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ILogger<RecuperationCommandService> logger) : IRecuperationCommandService
{
    /// <summary>Opens a recuperation and marks its component as IN_RECUPERATION in the same unit of work.</summary>
    public async Task<Result<Recuperation, OpenRecuperationError>> Handle(OpenRecuperationCommand command, CancellationToken cancellationToken = default)
    {
        if (await customerRepository.FindByIdAsync(command.CustomerId, cancellationToken) is null)
            return new Result<Recuperation, OpenRecuperationError>.Failure(OpenRecuperationError.CustomerNotFound);

        var component = await componentRepository.FindByIdAsync(command.ComponentId, cancellationToken);
        if (component is null)
            return new Result<Recuperation, OpenRecuperationError>.Failure(OpenRecuperationError.ComponentNotFound);

        if (component.CustomerId != command.CustomerId)
        {
            logger.LogWarning("Component {ComponentId} does not belong to customer {CustomerId}", command.ComponentId, command.CustomerId);
            return new Result<Recuperation, OpenRecuperationError>.Failure(OpenRecuperationError.ComponentNotOfCustomer);
        }

        if (await recuperationRepository.ExistsBySupplierOrganizationIdAndWorkOrderNumberAsync(command.SupplierOrganizationId, command.WorkOrderNumber, cancellationToken))
        {
            logger.LogWarning("Duplicate work order number {WorkOrderNumber} for organization {SupplierOrganizationId}", command.WorkOrderNumber, command.SupplierOrganizationId);
            return new Result<Recuperation, OpenRecuperationError>.Failure(OpenRecuperationError.DuplicateWorkOrderNumber);
        }

        try
        {
            var recuperation = new Recuperation(command);
            component.MarkInRecuperation();
            await recuperationRepository.AddAsync(recuperation, cancellationToken);
            componentRepository.Update(component);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Recuperation, OpenRecuperationError>.Success(recuperation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error opening recuperation {WorkOrderNumber}", command.WorkOrderNumber);
            return new Result<Recuperation, OpenRecuperationError>.Failure(OpenRecuperationError.UnexpectedError);
        }
    }

    public async Task<Result<Recuperation, UpdateRecuperationError>> Handle(UpdateRecuperationCommand command, CancellationToken cancellationToken = default)
    {
        var recuperation = await recuperationRepository.FindByIdAsync(command.RecuperationId, cancellationToken);
        if (recuperation is null) return new Result<Recuperation, UpdateRecuperationError>.Failure(UpdateRecuperationError.RecuperationNotFound);

        try
        {
            recuperation.Update(command);
            recuperationRepository.Update(recuperation);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Recuperation, UpdateRecuperationError>.Success(recuperation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating recuperation {RecuperationId}", command.RecuperationId);
            return new Result<Recuperation, UpdateRecuperationError>.Failure(UpdateRecuperationError.UnexpectedError);
        }
    }

    /// <summary>Changes the recuperation status; DELIVERED puts the component back IN_SERVICE in the same unit of work.</summary>
    public async Task<Result<Recuperation, ChangeRecuperationStatusError>> Handle(ChangeRecuperationStatusCommand command, CancellationToken cancellationToken = default)
    {
        var recuperation = await recuperationRepository.FindByIdAsync(command.RecuperationId, cancellationToken);
        if (recuperation is null) return new Result<Recuperation, ChangeRecuperationStatusError>.Failure(ChangeRecuperationStatusError.RecuperationNotFound);

        if (!recuperation.TryChangeStatus(command.Status))
        {
            logger.LogWarning("Invalid recuperation transition {From} -> {To} for {RecuperationId}", recuperation.Status, command.Status, command.RecuperationId);
            return new Result<Recuperation, ChangeRecuperationStatusError>.Failure(ChangeRecuperationStatusError.InvalidTransition);
        }

        try
        {
            if (command.Status == RecuperationStatus.Delivered)
            {
                var component = await componentRepository.FindByIdAsync(recuperation.ComponentId, cancellationToken);
                if (component is not null)
                {
                    component.MarkInService();
                    componentRepository.Update(component);
                }
            }

            recuperationRepository.Update(recuperation);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Recuperation, ChangeRecuperationStatusError>.Success(recuperation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error changing status of recuperation {RecuperationId}", command.RecuperationId);
            return new Result<Recuperation, ChangeRecuperationStatusError>.Failure(ChangeRecuperationStatusError.UnexpectedError);
        }
    }

    public async Task<Result<Recuperation, LinkSessionToRecuperationError>> Handle(LinkSessionToRecuperationCommand command, CancellationToken cancellationToken = default)
    {
        var recuperation = await recuperationRepository.FindByIdAsync(command.RecuperationId, cancellationToken);
        if (recuperation is null) return new Result<Recuperation, LinkSessionToRecuperationError>.Failure(LinkSessionToRecuperationError.RecuperationNotFound);

        try
        {
            recuperation.LinkSession(command.SessionId);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Recuperation, LinkSessionToRecuperationError>.Success(recuperation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error linking session {SessionId} to recuperation {RecuperationId}", command.SessionId, command.RecuperationId);
            return new Result<Recuperation, LinkSessionToRecuperationError>.Failure(LinkSessionToRecuperationError.UnexpectedError);
        }
    }
}
