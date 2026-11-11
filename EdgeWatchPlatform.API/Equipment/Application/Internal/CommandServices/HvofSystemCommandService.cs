using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
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
}
