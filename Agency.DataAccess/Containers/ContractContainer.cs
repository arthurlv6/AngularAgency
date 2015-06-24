using Agency.Data.Entities;
using Agency.DataAccess.BaseClasses;
using Agency.DataAccess.Components;
using Agency.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.Containers
{
    public interface IContractContainer
    {
        T FindDetail<T>(object keyValues) where T : class;
        T AddOneEntity<T>(T input) where T : class, new();
        void UpdateOneEntity<T>(T entity) where T : class, new();
        bool DeleteOneEntity<T>(object key) where T : class;
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class;
        void Dispose();
        DateTime GetNzTime();
        ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition, Expression<Func<T, TOrderBy>> orderBy, string[] includes = null, bool httpget = true) where T : class;
        List<Hospital> GetHospitals();
        List<Requirement> GetRequirements();
        List<VisaType> GetVisaTypes(long? parentId, bool dropdown = false, int i = 0);
    }
    public class ContractContainer : BaseOperations, IContractContainer
    {
        
    }
}
