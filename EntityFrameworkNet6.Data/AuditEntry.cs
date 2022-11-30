using EntityFrameworkNet6.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace EntityFrameworkNet6.Data
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entityEntry)
        {
            _entityEntry = entityEntry;
        }
        private readonly EntityEntry _entityEntry;
        public string Action { get; set; }
        public string TableName { get; set; }
        public string Username { get; set; }
        public Dictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; set; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit()
        {
            var audit = new Audit
            {
                DateTime = DateTime.Now,
                TableName = TableName,
                Username= Username,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                Action= Action
            };
            return audit;
        }

    }
}