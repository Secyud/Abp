using Microsoft.EntityFrameworkCore;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

public static class TenantsEfCoreQueryableExtensions
{
    public static IQueryable<Tenant> IncludeDetails(this IQueryable<Tenant> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.ConnectionStrings);
    }
}
