using Cortex.Mediator.Commands;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;

/// <summary>Pipeline behavior that commits the unit of work after a mediator command succeeds.</summary>
public class TransactionCommandBehavior<TCommand>(IUnitOfWork unitOfWork, ILogger<TransactionCommandBehavior<TCommand>> logger)
    : ICommandPipelineBehavior<TCommand> where TCommand : ICommand
{
    public async Task Handle(TCommand command, CommandHandlerDelegate next, CancellationToken ct)
    {
        await next();
        logger.LogDebug("Committing unit of work for {CommandType}", typeof(TCommand).Name);
        await unitOfWork.CompleteAsync(ct);
    }
}
