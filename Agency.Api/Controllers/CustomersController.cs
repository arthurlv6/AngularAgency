using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Agency.Data.DataContexts;
using Agency.Data.Entities;
using Agency.Api.ViewModels;
using AutoMapper;

namespace Agency.Api.Controllers
{
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {
        private AgencyDbContext db = new AgencyDbContext();
        // GET: api/Customers
        public IQueryable<ViewModelCustomer> GetCustomers()
        {
            var data= db.Customers.OrderBy(d=>d.CreateDate).Take(10).Include("Contracts.VisaType").Include("CustomerAudits.CustomerAuditFields").ToList();
            return getViewModelCustomerList(data).AsQueryable();
        }
        
        // GET: api/Customers/5
        [ResponseType(typeof(ViewModelCustomer))]
        public IHttpActionResult GetCustomer(string id)
        {
            if (id == "0")
            {
                return Ok(new ViewModelCustomer());
            }
            var data = db.Customers.Where(d => d.Name.Contains(id)).Take(10).Include("Contracts.VisaType").Include("CustomerAudits.CustomerAuditFields");
            return Ok(getViewModelCustomerList(data.ToList()).AsQueryable());
        }
        private IList<ViewModelCustomer> getViewModelCustomerList(IList<Customer> data)
        {
            Mapper.CreateMap<VisaType, ViewModelVisaType>();
            Mapper.CreateMap<Contract, ViewModelContract>();
            Mapper.CreateMap<CustomerAudit, ViewModelCustomerAudit>();
            Mapper.CreateMap<CustomerAuditField, ViewModelCustomerAuditField>();

            Mapper.CreateMap<Customer, ViewModelCustomer>();
            return  Mapper.Map<IList<Customer>, IList<ViewModelCustomer>>(data.ToList());
        }
        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(long id, ViewModelCustomer VMcustomer)
        {
            Mapper.CreateMap<ViewModelCustomer, Customer>().ForMember("Contracts", d => d.Ignore()).ForMember("CustomerAudits",d=>d.Ignore());
            Customer customer;
            try
            {
                 customer = Mapper.Map<ViewModelCustomer, Customer>(VMcustomer);
            }
            catch (Exception e)
            {
                throw e;
            }
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            //db.Entry(customer).State = EntityState.Modified;
            try
            {
                var existing = db.Customers.Find(id);
                db.Entry(existing).CurrentValues.SetValues(customer);
                CustomerDoAudit(existing, "system");
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        private void CustomerDoAudit(Customer entity, string user)
        {

            var audit = new CustomerAudit { Username = user, EventDtUtc = DateTime.Now, CustomerId = entity.Id };

            var dbEntityEntry = db.Entry(entity);
            foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
            {
                var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                if (original == null && current != null)
                    audit.CustomerAuditFields.Add(new CustomerAuditField()
                    {
                        Field = property.ToString(),
                        OldValue = "None",
                        NewValue = current.ToString()
                    });
                else if (original != null && current == null)
                    audit.CustomerAuditFields.Add(new CustomerAuditField()
                    {
                        Field = property.ToString(),
                        OldValue = original.ToString(),
                        NewValue = "None"
                    });
                else if (original != null && !original.Equals(current))
                    audit.CustomerAuditFields.Add(new CustomerAuditField()
                    {
                        Field = property.ToString(),
                        OldValue = original.ToString(),
                        NewValue = current.ToString()
                    });
            }
            if (audit.CustomerAuditFields.Count > 0)
            {
                db.CustomerAudits.Add(audit);
            }
        }
        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(long id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(long id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}