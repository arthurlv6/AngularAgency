using Agency.Data.Entities;
using Agency.DataAccess.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Agency.Api.Controllers
{
    [RoutePrefix("api/CommonInfo")]
    public class CommonInformationController : ApiController
    {
        IBaseOperations _baseOperations;
        public CommonInformationController(IBaseOperations baseOperations)
        {
            _baseOperations = new BaseOperations();
        }
        [Route("")]
        [Route("{SchoolTypes}")]
        public IQueryable<SchoolType> GetSchoolTypes(string SchoolTypes)
        {
            var data=_baseOperations.GetSchoolTypes(null, true);
            return data.AsQueryable();
        }
        [Route("")]
        [Route("{VisaTypes}")]
        public IQueryable<VisaType> GetVisaTypes(string VisaTypes)
        {
            var data = _baseOperations.GetVisaTypes(null, true);
            return data.AsQueryable();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _baseOperations.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
