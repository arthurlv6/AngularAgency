using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public sealed partial class Company
    {
        public Company()
        {
            Contracts = new HashSet<Contract>();
            Customers = new HashSet<Customer>();
            ApplicationUsers = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string TradingName { get; set; }
        [MaxLength(100)]
        public string GST { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }
        [MaxLength(200)]
        public string Logo { get; set; }
        [MaxLength(100)]
        public string PostalName { get; set; }
        [MaxLength(100)]
        public string PostalAddress { get; set; }
        [MaxLength(100)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string ContactName { get; set; }
        [MaxLength(256)]
        public string ContactEmail { get; set; }
        [MaxLength(200)]
        public string ContactAddress { get; set; }
        [MaxLength(50)]
        public string ContactOfficePhone { get; set; }
        [MaxLength(50)]
        public string ContactQQ { get; set; }
        [MaxLength(50)]
        public string ContactPhoneNumber { get; set; }
        [MaxLength(50)]
        public string ContactFaxNumber { get; set; }
        [MaxLength(50)]
        public string ContactMobileNumber { get; set; }
        public int NotificationVisaDays { get; set; }
        public int NotificationInsuranceDays { get; set; }
        public int NotificationCustomerDays { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }

}
