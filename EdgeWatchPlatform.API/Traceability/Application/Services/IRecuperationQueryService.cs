using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface IRecuperationQueryService
{
    Task<Recuperation?> Handle(GetRecuperationByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsBySupplierOrganizationIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsByCustomerIdQuery query, CancellationToken cancellationToken = default);
}
