using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public partial class Audit
    {
        public Audit()
        {
            this.AuditFields = new HashSet<AuditField>();
        }

        public long Id { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
        public System.DateTime EventDtUtc { get; set; }
        public string Username { get; set; }

        public virtual ICollection<AuditField> AuditFields { get; set; }
    }
}
