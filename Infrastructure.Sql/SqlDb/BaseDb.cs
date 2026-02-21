using System.Linq;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class BaseDb(PokerBunchDbContext db)
{
    protected async Task<int> GetBunchId(string slug) => await db.PbBunch
        .Where(o => o.Name == slug)
        .Select(o => o.BunchId)
        .FirstOrDefaultAsync();
}