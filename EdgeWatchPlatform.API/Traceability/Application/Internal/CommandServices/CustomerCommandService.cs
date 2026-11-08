using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.CommandServices;

public class CustomerCommandService(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ILogger<CustomerCommandService> logger) : ICustomerCommandService
{
    public async Task<Result<Customer, RegisterCustomerError>> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken = default)
    {
        if (await customerRepository.ExistsBySupplierOrganizationIdAndRucAsync(command.SupplierOrganizationId, command.Ruc, cancellationToken))
        {
            logger.LogWarning("Duplicate customer RUC {Ruc} for organization {SupplierOrganizationId}", command.Ruc, command.SupplierOrganizationId);
            return new Result<Customer, RegisterCustomerError>.Failure(RegisterCustomerError.DuplicateRuc);
        }

        try
        {
            var customer = new Customer(command);
            await customerRepository.AddAsync(customer, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Customer, RegisterCustomerError>.Success(customer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error registering customer {Ruc} for organization {SupplierOrganizationId}", command.Ruc, command.SupplierOrganizationId);
            return new Result<Customer, RegisterCustomerError>.Failure(RegisterCustomerError.UnexpectedError);
        }
    }

    public async Task<Result<Customer, UpdateCustomerError>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.FindByIdAsync(command.CustomerId, cancellationToken);
        if (customer is null) return new Result<Customer, UpdateCustomerError>.Failure(UpdateCustomerError.CustomerNotFound);

        try
        {
            customer.Update(command);
            customerRepository.Update(customer);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Customer, UpdateCustomerError>.Success(customer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating customer {CustomerId}", command.CustomerId);
            return new Result<Customer, UpdateCustomerError>.Failure(UpdateCustomerError.UnexpectedError);
        }
    }
}
