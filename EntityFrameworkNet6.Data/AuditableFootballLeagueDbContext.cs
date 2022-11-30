using EntityFrameworkNet6.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkNet6.Domain;

namespace EntityFrameworkNet6.Data
{
    public abstract class AuditableFootballLeagueDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }
        public async Task<int> SaveChangesAsync(string username)
        {
            var auditEntries = OnBeforeSaveChanges(username);
            var saveresult = await base.SaveChangesAsync();
            if (auditEntries != null || auditEntries.Count > 0)
            {
                await OnAfterSaveChanges(auditEntries);
            }

            return saveresult;
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                Audits.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }

        private List<AuditEntry> OnBeforeSaveChanges(string username)
        {
            var changes = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified || q.State == EntityState.Deleted);
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in changes)
            {
                var auditableObject = (BaseDomainObject)entry.Entity;
                auditableObject.DateModified = DateTime.Now;
                auditableObject.ModifiedBy = username;

                if (entry.State == EntityState.Added)
                {
                    auditableObject.DateCreated = DateTime.Now;
                    auditableObject.CreatedBy = username;
                }

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.GetTableName();
                auditEntry.Action = entry.State.ToString();
                auditEntry.Username = username;

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries.Where(x => x.HasTemporaryProperties == false))
            {
                Audits.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(x => x.HasTemporaryProperties).ToList();
        }
    }
}
