using Microservices.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Common.Interfaces
{
    public interface IApplicationDbContextBase
    {
        DbSet<DatabaseLocalizedString> LocalizedStrings { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}