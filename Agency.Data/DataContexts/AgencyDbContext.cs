using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agency.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Agency.Data.DataContexts
{
    public class AgencyDbContext : IdentityDbContext<ApplicationUser>
    {
        public AgencyDbContext()
            : base("Agency", throwIfV1Schema: false)
        {
        }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Company> Settings { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<VisaType> VisaTypes { get; set; }
        public DbSet<SchoolType> SchoolTypes { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditField> AuditFields { get; set; }
        public DbSet<CustomerAudit> CustomerAudits { get; set; }
        public DbSet<CustomerAuditField> CustomerAuditFields { get; set; }
        public static AgencyDbContext Create()
        {
            return new AgencyDbContext();
        }
    }
}
