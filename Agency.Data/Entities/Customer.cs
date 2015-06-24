using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public partial class Customer
    {
        public long Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        [MaxLength(50)]
        public string Number { get; set; }
        [MaxLength(50)]
        public string CurrentSchool { get; set; }
        [MaxLength(50)]
        public string CurrentPosition { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string QQId { get; set; }
        [MaxLength(50)]
        public string Prefer { get; set; }
        [MaxLength(50)]
        public string Reference { get; set; }
        [MaxLength(50)]
        public string PossibleVisaApply { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public string Detail { get; set; }
        public Company Company { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? NotificationDate { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<CustomerAudit> CustomerAudits { get; set; }
    }
}
