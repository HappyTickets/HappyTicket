using Application.Common.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Extensions
{
    internal static class EditableEntitiesExtensions
    {
        public static void PrepareAddedEntities(this ChangeTracker changeTracker, ICurrentUser user)
        {
            var entries = changeTracker
                .Entries<BaseEntity<long>>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.CreatedBy = 0; //user.Id.Value;
            }
        }

        public static void PrepareModifiedEntities(this ChangeTracker changeTracker, ICurrentUser user)
        {
            var entries = changeTracker
                .Entries<BaseEntity<long>>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
                entry.Entity.ModifiedBy = 0; //user.Id.Value;
            }
        }

        public static void PrepareDeletedEntities(this ChangeTracker changeTracker)
        {
            var entries = changeTracker
                   .Entries<SoftDeletableEntity<long>>()
                   .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Unchanged;

                entry.Entity.IsActive = false;
                entry.Entity.SoftDeleteCount += 1;
            }
        }
    }
}
