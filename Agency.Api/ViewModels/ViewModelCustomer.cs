using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agency.Api.ViewModels
{
    public partial class ViewModelCustomer
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
        public ApplicationUser ApplicationUser { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? NotificationDate { get; set; }
        public ICollection<ViewModelContract> Contracts { get; set; }
        public ICollection<ViewModelCustomerAudit> CustomerAudits { get; set; }
    }
}