using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.QueryServices;

public class RecuperationQueryService(IRecuperationRepository recuperationRepository) : IRecuperationQueryService
{
    public async Task<Recuperation?> Handle(GetRecuperationByIdQuery query, CancellationToken cancellationToken = default) =>
        await recuperationRepository.FindByIdAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsQuery query, CancellationToken cancellationToken = default) =>
        await recuperationRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsBySupplierOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await recuperationRepository.FindBySupplierOrganizationIdAsync(query.SupplierOrganizationId, cancellationToken);

    public async Task<IEnumerable<Recuperation>> Handle(GetAllRecuperationsByCustomerIdQuery query, CancellationToken cancellationToken = default) =>
        await recuperationRepository.FindByCustomerIdAsync(query.CustomerId, cancellationToken);
}
