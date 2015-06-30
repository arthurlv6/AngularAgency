using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Agency.Data.DataContexts;
using Agency.Data.Entities;
using Agency.DataAccess.BaseClasses;
using Agency.Api.ViewModels;
using AutoMapper;

namespace Agency.Api.Controllers
{
    public class SchoolTypesController : ApiController
    {
        IBaseOperations _baseOperations;
        public SchoolTypesController(IBaseOperations baseOperations)
        {
            _baseOperations = baseOperations;
        }
        // GET: api/SchoolTypes
        public IQueryable<ViewModelSchoolType> GetSchoolTypes()
        {
            Mapper.CreateMap<SchoolType, ViewModelSchoolType>();//.ForSourceMember("Schools",d=>d.Ignore()).ForSourceMember("Parent",d=>d.Ignore());
            var data= _baseOperations.GetSchoolTypes(null, true).AsQueryable();
            return Mapper.Map<IList<SchoolType>, IList<ViewModelSchoolType>>(data.ToList()).AsQueryable();
        }

        // GET: api/SchoolTypes/5
        [ResponseType(typeof(SchoolType))]
        public IHttpActionResult GetSchoolType(long id)
        {
            SchoolType schoolType = _baseOperations.FindDetail<SchoolType>(id);
            if (schoolType == null)
            {
                return NotFound();
            }

            return Ok(schoolType);
        }
        /*
        // PUT: api/SchoolTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSchoolType(long id, SchoolType schoolType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != schoolType.Id)
            {
                return BadRequest();
            }

            _baseOperations.Entry(schoolType).State = EntityState.Modified;

            try
            {
                _baseOperations.SaveChanges();
            }
            catch (_baseOperationsUpdateConcurrencyException)
            {
                if (!SchoolTypeExists(id))
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

        // POST: api/SchoolTypes
        [ResponseType(typeof(SchoolType))]
        public IHttpActionResult PostSchoolType(SchoolType schoolType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _baseOperations.SchoolTypes.Add(schoolType);
            _baseOperations.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = schoolType.Id }, schoolType);
        }

        // DELETE: api/SchoolTypes/5
        [ResponseType(typeof(SchoolType))]
        public IHttpActionResult DeleteSchoolType(long id)
        {
            SchoolType schoolType = _baseOperations.SchoolTypes.Find(id);
            if (schoolType == null)
            {
                return NotFound();
            }

            _baseOperations.SchoolTypes.Remove(schoolType);
            _baseOperations.SaveChanges();

            return Ok(schoolType);
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _baseOperations.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SchoolTypeExists(long id)
        {
            return _baseOperations.Get<SchoolType>(t=>true).Count(e => e.Id == id) > 0;
        }
    }
}