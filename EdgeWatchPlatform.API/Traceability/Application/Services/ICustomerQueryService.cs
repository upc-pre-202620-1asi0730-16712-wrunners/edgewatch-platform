using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface ICustomerQueryService
{
    Task<Customer?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> Handle(GetAllCustomersBySupplierOrganizationIdQuery query, CancellationToken cancellationToken = default);
}
