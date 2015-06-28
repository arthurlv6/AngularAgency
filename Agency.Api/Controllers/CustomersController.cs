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
        public async Task<IHttpActionResult> PutCustomer(long id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
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