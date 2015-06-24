using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agency.Data.Entities;
using Agency.DataAccess.ViewModels;

namespace Agency.DataAccess.BaseClasses
{
    public interface IBaseOperations
    {
        T AddOneEntity<T>(T input) where T : class, new();
        bool DeleteOneEntity<T>(object key) where T : class;
        void Dispose();
        void DoAudit<T>(T entity, long id, string user) where T : class;
        T FindDetail<T>(object keyValues) where T : class;
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class;
        List<Hospital> GetHospitals();
        DateTime GetNzTime();
        List<Requirement> GetRequirements();
        List<SchoolType> GetSchoolTypes(long? parentId, bool dropdown = false, int i = 0);
        ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition, Expression<Func<T, TOrderBy>> orderBy, string[] includes = null, bool httpget = true) where T : class;
        List<VisaType> GetVisaTypes(long? parentId, bool dropdown = false, int i = 0);
        void RecordException(ErrorLog input);
        void UpdateOneEntity<T>(T entity) where T : class, new();
        DateTime NewZealandTime { get; }
    }
}