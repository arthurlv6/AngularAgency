using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Agency.Data.Entities;
using Agency.DataAccess.BaseClasses;
using Agency.DataAccess.ViewModels;

using Microsoft.AspNet.Identity.EntityFramework;
using Agency.DataAccess.Components;

namespace Agency.DataAccess.Containers
{
    public interface ISettingContainer
    {
        void OperatorDelete(string id);
        void OperatorSetupRole(string id, string[] roles);
        void RecordException(ErrorLog input);
        T FindDetail<T>(object keyValues) where T : class;
        T AddOneEntity<T>(T input) where T : class, new();
        void UpdateOneEntity<T>(T entity) where T : class, new();
        bool DeleteOneEntity<T>(object key) where T : class;
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class;
        List<SchoolType> GetSchoolTypes(long? parentId, bool dropdown = false, int i = 0);
        void Dispose();
        DateTime GetNzTime();
        List<VisaType> GetVisaTypes(long? parentId, bool dropdown = false, int i = 0);
        //ConditionViewModel<T> GetSchools<T, TOrderBy>(ConditionViewModel<T> input, Expression<Func<T, TOrderBy>> OrderBy, string[] includes, bool httpget = true) where T : class;
        List<Hospital> GetHospitals();
        List<Requirement> GetRequirements();
        ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition, Expression<Func<T, TOrderBy>> orderBy, string[] includes = null, bool httpget = true) where T : class;
    }

    public class SettingContainer : BaseOperations, ISettingContainer
    {
        #region application user
        public void OperatorDelete(string id)
        {
            try
            {
                var user = _db.Users.Find(id);
                IdentityUserRole[] existRoles = user.Roles.ToList().ToArray();
                if (existRoles.Length > 0)
                {
                    foreach (var role in existRoles)
                    {
                        user.Roles.Remove(role);
                    }
                }
                user.Status = "Deleted";
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                RecordException(new ErrorLog
                {
                    Message = e.Message,
                    MessageDump = e.StackTrace,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name
                });
                throw new Exception("fail to delete.");
            }
        }
        public void OperatorSetupRole(string id, string[] roles)
        {
            try
            {
                var user = _db.Users.Find(id);
                user.UpdateDatetime = NewZealandTime;
                IdentityUserRole[] existRoles=user.Roles.ToList().ToArray();
                if (existRoles.Length > 0)
                {
                    foreach (var role in existRoles)
                    {
                        user.Roles.Remove(role);
                    }
                }
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        user.Roles.Add(new IdentityUserRole { RoleId = role });
                    }
                }
                
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                RecordException(new ErrorLog
                {
                    Message = e.Message,
                    MessageDump = e.StackTrace,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name
                });
                throw new Exception("fail to setup roles.");
            }
        }
        #endregion
    }
}
