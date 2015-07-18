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
using System.Web.Http.Cors;
using System.Web.OData;
using AutoMapper;
using Agency.Api.ViewModels;
using Agency.DataAccess.BaseClasses;
using Agency.DataAccess.ViewModels;
using System.Linq.Expressions;
namespace Agency.Api.Controllers
{
    [RoutePrefix("api/Schools")]
    public class SchoolsController : ApiController
    {
        private AgencyDbContext db = new AgencyDbContext();
        // GET: api/Schools
        public IHttpActionResult GetSchoolsBySearchAndPagination(string search,int pageNumber)
        {
            try
            {
                Expression<Func<School, bool>> fun;
                if (string.IsNullOrEmpty(search))
                {
                    fun = d => true;
                }
                else
                {
                    fun = d => d.Name.Contains(search);
                }
                var total = db.Schools.Where(fun).Count();
                var totalPages = total / 10 + (total % 10 > 0 ? 1 : 0);
                if (pageNumber > totalPages)
                {
                    pageNumber = 1;
                }

                var data = db.Schools.Where(fun).OrderByDescending(d => d.CreateDate).Skip(10*(pageNumber-1)).Take(10).Include(d => d.SchoolType).ToList();
                return Ok(getViewModelSchoolList(data));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        // GET: api/Schools/5
        [ResponseType(typeof(ViewModelTotalCount))]
        public IHttpActionResult GetSchoolTotalCount(string condition)
        {
            Expression<Func<School, bool>> fun;
            if (string.IsNullOrEmpty(condition))
            {
                fun = d => true;
            }
            else
            {
                fun = d => d.Name.Contains(condition);
            }
            return Ok(new ViewModelTotalCount() { count = db.Schools.Where(fun).Count() });
        }

        // GET: api/Schools/5
        [ResponseType(typeof(ViewModelSchool))]
        public IHttpActionResult GetSchool(string id)
        {
            if (id == "0")
            {
                return Ok( new School() { UserId = "12ae3ecb-c59d-4ebf-9826-2ccbeb9f7838", SchoolTypeId = 1, CreateDate = DateTime.Now });
            }
            else
            {
                List<School> schools = db.Schools.Include(d => d.SchoolType).Where(d=>d.Name.Contains(id)).ToList();
                return Ok(getViewModelSchoolList(schools));
            }
        }

        // PUT: api/Schools/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSchool(long id, ViewModelSchool viewModelschool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != viewModelschool.Id)
            {
                return BadRequest();
            }
            Mapper.CreateMap<ViewModelSchool,School>().ForMember("SchoolType",d=>d.Ignore());
            var school=Mapper.Map<ViewModelSchool,School>(viewModelschool);
            db.Entry(school).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolExists(id))
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
        // POST: api/Schools
        [ResponseType(typeof(ViewModelSchool))]
        public async Task<IHttpActionResult> PostSchool(ViewModelSchool viewModelschool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Mapper.CreateMap<ViewModelSchool, School>().ForMember("SchoolType", d => d.Ignore());
            var school = Mapper.Map<ViewModelSchool, School>(viewModelschool);
            db.Schools.Add(school);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = school.Id }, school);
        }
        // DELETE: api/Schools/5
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> DeleteSchool(long id)
        {
            School school = await db.Schools.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            db.Schools.Remove(school);
            await db.SaveChangesAsync();

            return Ok(school);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private IList<ViewModelSchool> getViewModelSchoolList(IList<School> data)
        {
            Mapper.CreateMap<School, ViewModelSchool>();
            Mapper.CreateMap<SchoolType, ViewModelSchoolType>();
            IList<ViewModelSchool> viewModelList = Mapper.Map<IList<School>, IList<ViewModelSchool>>(data.ToList());
            return viewModelList;
        }
        private bool SchoolExists(long id)
        {
            return db.Schools.Count(e => e.Id == id) > 0;
        }
    }
}