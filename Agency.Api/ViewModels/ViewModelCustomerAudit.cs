using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Api.ViewModels
{
    public class ViewModelCustomerAudit
    {
        public long Id { get; set; }
        public DateTime EventDtUtc { get; set; }
        public string Username { get; set; }
        public virtual ICollection<ViewModelCustomerAuditField> CustomerAuditFields { get; set; }
    }
}
