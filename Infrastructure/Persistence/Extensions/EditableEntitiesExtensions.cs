using Application.Common.Interfaces;
using Domain.Entities.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions
{
    internal static class EditableEntitiesExtensions
    {
        public static void PrepareAddedEntities(this ChangeTracker changeTracker, ICurrentUser user)
        {
            var entries = changeTracker
                .Entries<BaseEntity<object>>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.CreatedBy = user.Id.Value;
            }
        }

        public static void PrepareModifiedEntities(this ChangeTracker changeTracker, ICurrentUser user)
        {
            var entries = changeTracker
                .Entries<BaseEntity<object>>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
                entry.Entity.ModifiedBy = user.Id.Value;
            }
        }

        public static void PrepareDeletedEntities(this ChangeTracker changeTracker)
        {
            var entries = changeTracker
                   .Entries<SoftDeletableEntity<object>>()
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
