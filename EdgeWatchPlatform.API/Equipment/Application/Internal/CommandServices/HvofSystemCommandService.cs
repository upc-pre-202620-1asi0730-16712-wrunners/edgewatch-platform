using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Errors;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.CommandServices;

public class HvofSystemCommandService(
    IHvofSystemRepository hvofSystemRepository,
    IUnitOfWork unitOfWork,
    ILogger<HvofSystemCommandService> logger) : IHvofSystemCommandService
{
    public async Task<Result<HvofSystem>> Handle(RegisterHvofSystemCommand command, CancellationToken cancellationToken = default)
    {
        if (await hvofSystemRepository.ExistsByOrganizationIdAndCodeAsync(command.OrganizationId, command.Code, cancellationToken))
        {
            logger.LogWarning("Duplicate HVOF system code {Code} for organization {OrganizationId}", command.Code, command.OrganizationId);
            return Result<HvofSystem>.Failure(EquipmentErrors.DuplicateHvofSystemCode, EquipmentError.DuplicateHvofSystemCode);
        }

        try
        {
            var hvofSystem = new HvofSystem(command);
            await hvofSystemRepository.AddAsync(hvofSystem, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofSystem>.Success(hvofSystem);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error registering HVOF system {Code} for organization {OrganizationId}", command.Code, command.OrganizationId);
            return Result<HvofSystem>.Failure(EquipmentErrors.DatabaseError, EquipmentError.DatabaseError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error registering HVOF system {Code}", command.Code);
            return Result<HvofSystem>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<HvofSubsystem>> Handle(AddSubsystemToHvofSystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<HvofSubsystem>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);

        try
        {
            var subsystem = new HvofSubsystem(command.SubsystemType, command.Name, command.Alias, command.Parameters);
            if (!hvofSystem.TryAddSubsystem(subsystem))
                return Result<HvofSubsystem>.Failure(EquipmentErrors.DuplicateSubsystemAlias, EquipmentError.DuplicateSubsystemAlias);
            hvofSystem.RecordSubsystemAdded(subsystem);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofSubsystem>.Success(subsystem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error adding subsystem {Alias} to HVOF system {HvofSystemId}", command.Alias, command.HvofSystemId);
            return Result<HvofSubsystem>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<HvofPart>> Handle(AddPartToSubsystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<HvofPart>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        var subsystem = hvofSystem.FindSubsystemById(command.SubsystemId);
        if (subsystem is null) return Result<HvofPart>.Failure(EquipmentErrors.SubsystemNotFound, EquipmentError.SubsystemNotFound);

        try
        {
            var part = new HvofPart(command.PartType, command.SerialNumber, command.Manufacturer);
            if (!subsystem.TryAddPart(part))
                return Result<HvofPart>.Failure(EquipmentErrors.DuplicatePartSerialNumber, EquipmentError.DuplicatePartSerialNumber);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofPart>.Success(part);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error adding part {SerialNumber} to subsystem {SubsystemId}", command.SerialNumber, command.SubsystemId);
            return Result<HvofPart>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<HvofSystem>> Handle(UpdateHvofSystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<HvofSystem>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        try
        {
            hvofSystem.Update(command);
            hvofSystemRepository.Update(hvofSystem);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofSystem>.Success(hvofSystem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating HVOF system {HvofSystemId}", command.HvofSystemId);
            return Result<HvofSystem>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<HvofSubsystem>> Handle(UpdateSubsystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<HvofSubsystem>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        var subsystem = hvofSystem.FindSubsystemById(command.SubsystemId);
        if (subsystem is null) return Result<HvofSubsystem>.Failure(EquipmentErrors.SubsystemNotFound, EquipmentError.SubsystemNotFound);
        if (hvofSystem.HasOtherSubsystemWithAlias(command.SubsystemId, command.Alias))
            return Result<HvofSubsystem>.Failure(EquipmentErrors.DuplicateSubsystemAlias, EquipmentError.DuplicateSubsystemAlias);
        try
        {
            subsystem.Update(command.SubsystemType, command.Name, command.Alias, command.Parameters);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofSubsystem>.Success(subsystem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating subsystem {SubsystemId}", command.SubsystemId);
            return Result<HvofSubsystem>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<Controller>> Handle(UpdateControllerCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<Controller>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        var controller = hvofSystem.FindControllerById(command.ControllerId);
        if (controller is null) return Result<Controller>.Failure(EquipmentErrors.ControllerNotFound, EquipmentError.ControllerNotFound);
        try
        {
            controller.Update(command.DeviceType, command.SerialNumber, command.Manufacturer, command.Model, command.PartNumber, command.IpAddress,
                command.Port, command.Rack, command.Slot, command.EndpointUrl, command.Protocols);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Controller>.Success(controller);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating controller {ControllerId}", command.ControllerId);
            return Result<Controller>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<HvofSystem>> Handle(RemovePartFromSubsystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<HvofSystem>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        if (hvofSystem.FindSubsystemById(command.SubsystemId) is null)
            return Result<HvofSystem>.Failure(EquipmentErrors.SubsystemNotFound, EquipmentError.SubsystemNotFound);
        if (!hvofSystem.TryRemovePart(command.SubsystemId, command.PartId))
            return Result<HvofSystem>.Failure(EquipmentErrors.PartNotFound, EquipmentError.PartNotFound);
        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<HvofSystem>.Success(hvofSystem);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error removing part {PartId}", command.PartId);
            return Result<HvofSystem>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<Controller>> Handle(AddControllerToHvofSystemCommand command, CancellationToken cancellationToken = default)
    {
        var hvofSystem = await hvofSystemRepository.FindByIdWithCompositionAsync(command.HvofSystemId, cancellationToken);
        if (hvofSystem is null) return Result<Controller>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);

        try
        {
            var controller = new Controller(command.ControllerNumber, command.DeviceType, command.SerialNumber, command.Manufacturer, command.Model,
                command.PartNumber, command.IpAddress, command.Port, command.Rack, command.Slot, command.EndpointUrl, command.Protocols);
            if (!hvofSystem.TryAddController(controller))
                return Result<Controller>.Failure(EquipmentErrors.DuplicateControllerNumber, EquipmentError.DuplicateControllerNumber);
            hvofSystem.RecordControllerAdded(controller);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Controller>.Success(controller);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error adding controller {ControllerNumber} to HVOF system {HvofSystemId}", command.ControllerNumber, command.HvofSystemId);
            return Result<Controller>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }
}
