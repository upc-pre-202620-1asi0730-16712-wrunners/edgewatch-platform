using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default) =>
        await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default) =>
        await Context.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);

    public async Task<IEnumerable<User>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<User>().Where(u => u.OrganizationId == organizationId).OrderBy(u => u.FullName).ToListAsync(cancellationToken);
}
