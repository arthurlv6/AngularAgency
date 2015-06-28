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
        [EnableQuery()]
        [ResponseType(typeof(School))]
        public IHttpActionResult GetSchools()
        {
            try
            {
                //var data=db.Schools.AsQueryable();
                //Mapper.CreateMap<School, ViewModelSchool>();
                //IList<ViewModelSchool> viewModelList =Mapper.Map<IList<School>, IList<ViewModelSchool>>(data.ToList());

                return Ok(db.Schools.Where(d=>d.SchoolType.Id==1).AsQueryable());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Schools/5
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> GetSchool(long id)
        {
            School school = await db.Schools.FindAsync(id);
            if (id > 0)
            {
                if (school == null)
                {
                    return NotFound();
                }
            }else
            {
                school = new School() { UserId= "12ae3ecb-c59d-4ebf-9826-2ccbeb9f7838",SchoolTypeId=1,CreateDate=DateTime.Now };
            }
            return Ok(school);
        }

        // PUT: api/Schools/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSchool(long id, School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != school.Id)
            {
                return BadRequest();
            }

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
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> PostSchool(School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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