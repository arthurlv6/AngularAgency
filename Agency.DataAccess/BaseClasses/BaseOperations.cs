using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using Agency.Data.DataContexts;
using Agency.Data.Entities;
using Agency.DataAccess.Components;
using Agency.DataAccess.ViewModels;
using Agency.DataAccess.BaseClasses;

namespace Agency.DataAccess.BaseClasses
{

    public class BaseOperations : IDisposable, IBaseOperations
    {
        protected readonly AgencyDbContext _db;
        public BaseOperations()
        {
            _db=new AgencyDbContext();
        }
        public DateTime NewZealandTime
        {
            get
            {
                DateTime serverTime = DateTime.Now;
                TimeZoneInfo timeZone1 = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
                return TimeZoneInfo.ConvertTime(serverTime, timeZone1);
            }
        }

        public DateTime GetNzTime()
        {
            return NewZealandTime;
        }
        public void RecordException(ErrorLog input)
        {
            foreach (DbEntityEntry entry in _db.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    // If the EntityState is the Deleted, reload the date from the database.   
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
            if (string.IsNullOrEmpty(input.Message))
                input.Message = "Failed to catch the error message.";
            if (string.IsNullOrEmpty(input.MessageDump))
                input.Message = "Failed to catch the error stack message.";
            if (string.IsNullOrEmpty(input.DeviceId))
                input.DeviceId = "Unknow deviceId.";

            input.EventUtc = NewZealandTime;
            _db.ErrorLogs.Add(input);
            _db.SaveChanges();
        }
        public virtual ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition,Expression<Func<T, TOrderBy>> orderBy,  string[] includes = null, bool httpget = true) where T : class
        {
            var result = new ConditionViewModel<T>();
            SortOrder sortOrder = SortOrder.Descending;
            if (httpget)
            {
                if (condition.CurrentPage == 0 || condition.PerPageSize==0)
                {
                    condition.CurrentPage = 1;
                    condition.PerPageSize = 10;
                }
            }
            else
            {
                if (condition.CurrentPage == 0) //search
                {
                    condition.PerPageSize = 10;
                    condition.CurrentPage = 1;
                }
            }
            if (condition.ChangeOrderDirection)
            {
                if (condition.OrderDirection == "desc")
                {
                    sortOrder = SortOrder.Descending;
                    condition.OrderDirection = "asc";
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                    condition.OrderDirection = "desc";
                }
            }
            else
            {
                if (condition.OrderDirection == "desc")
                {
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                }
            }

            IQueryable<T> group;
            if (condition.Func == null)
            {
                condition.Func = d => true;
            }
            int total = _db.Set<T>().Count(condition.Func);
            var totalPages = total / condition.PerPageSize + (total % condition.PerPageSize > 0 ? 1 : 0);
            if (condition.CurrentPage > totalPages)
            {
                condition.CurrentPage = 1;
            }
            if (sortOrder == SortOrder.Ascending)
            {
                group =
                    _db.Set<T>()
                        .Where(condition.Func)
                        .OrderBy(orderBy)
                        .Skip((condition.CurrentPage - 1) * condition.PerPageSize)
                        .Take(condition.PerPageSize);
            }
            else
            {
                group =
                    _db.Set<T>()
                        .Where(condition.Func)
                        .OrderByDescending(orderBy)
                        .Skip((condition.CurrentPage - 1) * condition.PerPageSize)
                        .Take(condition.PerPageSize);
            }
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    group=group.Include(item);
                }
            }
            result.Data = group.ToList();
            result.TotalPages = totalPages == 0 ? 1 : totalPages;
            result.PerPageSize = condition.PerPageSize;
            result.CurrentPage = condition.CurrentPage;
            result.OrderDirection = condition.OrderDirection;
            result.Search = condition.Search;
            return result;
        }

        public T FindDetail<T>(object keyValues) where T : class
        {
            return _db.Set<T>().Find(keyValues);
        }
        public T AddOneEntity<T>(T input) where T : class, new()
        {
            try
            {
                _db.Set<T>().Add(input);
                _db.SaveChanges();
                return input;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void UpdateOneEntity<T>(T entity) where T : class, new()
        {
            try
            {
                if (entity == null)
                    throw new Exception("entity is null");
                _db.Set<T>().Attach(entity);
                
                //DoAudit(entity, 1, "someone");
                _db.Entry(entity).State = EntityState.Modified;
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
        public bool DeleteOneEntity<T>(object key) where T : class
        {
            try
            {
                T existing = _db.Set<T>().Find(key);
                _db.Set<T>().Remove(existing);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class
        {
            return _db.Set<T>().Where(criteria).ToList();
        }
        protected void ThrowOperationException(Action action)
        {
            try
            {
               action.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb=new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        //Console.WriteLine();
                        sb.Append(string.Format("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                    Console.WriteLine();
                }
                throw new DbEntityValidationException(sb.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Please contact your database manager.");
            }

        }
        protected List<T> ThrowOperationException<T>(Func<List<T>> func) where T : class,new()
        {
            try
            {
               return func.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        //Console.WriteLine();
                        sb.Append(string.Format("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                    Console.WriteLine();
                }
                throw new DbEntityValidationException(sb.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Please contact your database manager.");
            }

        }
        public List<SchoolType> GetSchoolTypes(long? parentId, bool dropdown = false, int i = 0)
        {
            var source = CacheHelper.GetCache<SchoolType>(CacheType.SchoolType);
            var rzt = new List<SchoolType>();
            var types = source.Where(d => d.ParentId == parentId);
            int temp = i + 1;
            foreach (var type in types)
            {
                if (dropdown)
                {
                    var name = "-".Repeat(temp) + type.Name;
                    rzt.Add(new SchoolType() { Id = type.Id, Name = name, ParentId = type.ParentId, Parent = type.Parent });
                }
                else
                {
                    rzt.Add(type);
                }

                var subItems = GetSchoolTypes(type.Id, dropdown, temp);
                foreach (var subItem in subItems)
                {
                    if (dropdown)
                    {
                        var name = "-".Repeat(temp) + subItem.Name;
                        rzt.Add(new SchoolType() { Id = subItem.Id, Name = name, ParentId = subItem.ParentId, Parent = subItem.Parent });
                    }
                    else
                    {
                        rzt.Add(subItem);
                    }
                }
            }
            return rzt;
        }
        public List<VisaType> GetVisaTypes(long? parentId, bool dropdown = false, int i = 0)
        {
            var source = CacheHelper.GetCache<VisaType>(CacheType.VisaType);
            var rzt = new List<VisaType>();
            var types = source.Where(d => d.ParentId == parentId);
            int temp = i + 1;
            foreach (var type in types)
            {
                if (dropdown)
                {
                    var name = "-".Repeat(temp) + type.Name;
                    rzt.Add(new VisaType() { Id = type.Id, Name = name, ParentId = type.ParentId, Parent = type.Parent });
                }
                else
                {
                    rzt.Add(type);
                }
                var subItems = GetVisaTypes(type.Id, dropdown, temp);
                foreach (var subItem in subItems)
                {
                    if (dropdown)
                    {
                        var name = "-".Repeat(temp) + subItem.Name;
                        rzt.Add(new VisaType() { Id = subItem.Id, Name = name, ParentId = subItem.ParentId, Parent = subItem.Parent });
                    }
                    else
                    {
                        rzt.Add(subItem);
                    }
                }
            }
            return rzt;
        }
        public List<Hospital> GetHospitals()
        {
            return CacheHelper.GetCache<Hospital>(CacheType.Hospital).OrderBy(d=>d.SortOrder).ToList();
        }
        public List<Requirement> GetRequirements()
        {
            return CacheHelper.GetCache<Requirement>(CacheType.Requirement).OrderBy(d => d.SortOrder).ToList();
        }
        public void DoAudit<T>(T entity, long id, string user) where T : class
        {
            var entityName = "";
            if (entity.GetType().Name.IndexOf("_", System.StringComparison.Ordinal) > 0)
            {
                entityName = entity.GetType()
                    .Name.Substring(0, entity.GetType().Name.IndexOf("_", System.StringComparison.Ordinal));
            }
            else
            {
                entityName = entity.GetType().Name;
            }
            var audit = new Audit { Username = user, EventDtUtc = NewZealandTime, EntityId = 0, Entity = entityName };

            var dbEntityEntry = _db.Entry(entity);
            foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
            {
                var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                if (original == null && current != null)
                    audit.AuditFields.Add(new AuditField()
                    {
                        Field = property.ToString(),
                        OldValue = "None",
                        NewValue = current.ToString()
                    });
                else if (original != null && current == null)
                    audit.AuditFields.Add(new AuditField()
                    {
                        Field = property.ToString(),
                        OldValue = original.ToString(),
                        NewValue = "None"
                    });
                else if (original != null && !original.Equals(current))
                    audit.AuditFields.Add(new AuditField()
                    {
                        Field = property.ToString(),
                        OldValue = original.ToString(),
                        NewValue = current.ToString()
                    });
            }
            if (audit.AuditFields.Count > 0)
            {
                audit.EntityId = id;
                _db.Audits.Add(audit);
            }
        }
        
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
