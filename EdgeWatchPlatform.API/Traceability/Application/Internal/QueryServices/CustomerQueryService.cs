using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.QueryServices;

public class CustomerQueryService(ICustomerRepository customerRepository) : ICustomerQueryService
{
    public async Task<Customer?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default) =>
        await customerRepository.FindByIdAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken = default) =>
        await customerRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersBySupplierOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await customerRepository.FindBySupplierOrganizationIdAsync(query.SupplierOrganizationId, cancellationToken);
}
