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

namespace Agency.Api.Controllers
{
    public class SchoolsController : ApiController
    {
        private AgencyDbContext db = new AgencyDbContext();
        // GET: api/Schools
        public IHttpActionResult GetSchools()
        {
            try
            {
                var data = db.Schools.Include(d=>d.SchoolType).OrderByDescending(d => d.CreateDate).ToList();
                return Ok(getViewModelSchoolList(data));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
                List<School> schools = db.Schools.Where(d=>d.Name.Contains(id)).ToList();

                return Ok(getViewModelSchoolList(schools));
            }
            
        }
        private IList<ViewModelSchool> getViewModelSchoolList(IList<School> data)
        {
            Mapper.CreateMap<School, ViewModelSchool>();
            Mapper.CreateMap<SchoolType, ViewModelSchoolType>();
            IList<ViewModelSchool> viewModelList = Mapper.Map<IList<School>, IList<ViewModelSchool>>(data.ToList());
            return viewModelList;
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

        private bool SchoolExists(long id)
        {
            return db.Schools.Count(e => e.Id == id) > 0;
        }
    }
}