using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public partial class CustomerAudit
    {
        public CustomerAudit()
        {
            this.CustomerAuditFields = new HashSet<CustomerAuditField>();
        }
        public long Id { get; set; }
        public DateTime EventDtUtc { get; set; }
        [MaxLength(256)]
        public string Username { get; set; }
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CustomerAuditField> CustomerAuditFields { get; set; }
    }
}
