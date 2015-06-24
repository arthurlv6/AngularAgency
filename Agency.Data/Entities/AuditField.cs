using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public partial class AuditField
    {
        public long Id { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public long AuditId { get; set; }
        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; }
    }
}
