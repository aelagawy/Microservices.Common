using Microservices.Common.Enums;
using Microservices.Common.Interfaces;
using Microservices.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Microservices.Common
{
    public abstract class ApplicationDbContextBase : DbContext, IApplicationDbContextBase
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly DateTime _dateTime = DateTime.Now;

        public DbSet<DatabaseLocalizedString> LocalizedStrings { get; set; }

        public ApplicationDbContextBase(DbContextOptions options, ICurrentUser currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUser.UserId;
                        entry.Entity.Created = _dateTime;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUser.UserId;
                        entry.Entity.LastModified = _dateTime;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.RowStatus = RowStatus.Deleted;
                        entry.Entity.LastModifiedBy = _currentUser.UserId;
                        entry.Entity.LastModified = _dateTime;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DatabaseLocalizedString>().HasKey(x => x.Key);

            foreach (var entityType in builder.Model.GetEntityTypes()
                .Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType).Property<RowStatus>("RowStatus");
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.NotEqual(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(RowStatus) }, parameter, Expression.Constant("RowStatus")),
                Expression.Constant(RowStatus.Deleted));
                var expression = Expression.Lambda(body, parameter);
                builder.Entity(entityType.ClrType).HasQueryFilter(expression);
            }
        }
    }
}