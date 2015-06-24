using Agency.Data.Entities;
using Agency.DataAccess.BaseClasses;
using Agency.DataAccess.Components;
using Agency.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.Containers
{
    public interface ICustomerContainer
    {
        T FindDetail<T>(object keyValues) where T : class;
        T AddOneEntity<T>(T input) where T : class, new();
        void UpdateOneEntity<T>(T entity) where T : class, new();
        bool DeleteOneEntity<T>(object key) where T : class;
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class;
        void Dispose();
        DateTime GetNzTime();
        List<Hospital> GetHospitals();
        List<Requirement> GetRequirements();
        ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition, Expression<Func<T, TOrderBy>> orderBy, string[] includes = null, bool httpget = true) where T : class;
        void Delete(long id);
        void Update(Customer entity, string userName);
    }
    public class CustomerContainer : BaseOperations, ICustomerContainer
    {
        public void Delete(long id)
        {
            _db.Customers.Find(id).Deleted = true;
            _db.SaveChanges();
        }
        public void Update(Customer entity,string userName)
        {
            try
            {
                if (entity == null)
                    throw new Exception("entity is null");
                var existing=_db.Customers.Find(entity.Id);
                _db.Entry(existing).CurrentValues.SetValues(entity);
                CustomerDoAudit(existing, userName);
                _db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        Console.WriteLine("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                    Console.WriteLine();
                }
                throw new Exception("entity is null");
            }
            catch (Exception)
            {
                throw new Exception("entity is null");
            }
        }
        private void CustomerDoAudit(Customer entity, string user)
        {
            
            var audit = new CustomerAudit { Username = user, EventDtUtc = NewZealandTime, CustomerId=entity.Id };

            var dbEntityEntry = _db.Entry(entity);
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
                _db.CustomerAudits.Add(audit);
            }
        }
        public void Test()
        {
            var result = _db.Customers.Where(d => d.Id ==13).SelectMany(c => c.Contracts).Select(r => r.SignDate);
        }
    }
}
