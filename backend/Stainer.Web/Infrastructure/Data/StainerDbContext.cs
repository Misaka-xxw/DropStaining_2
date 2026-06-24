using Microsoft.EntityFrameworkCore;

namespace Stainer.Web.Infrastructure.Data;

public sealed class StainerDbContext(DbContextOptions<StainerDbContext> options) : DbContext(options)
{
}
