using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using NinjaWebAPI.DTOs;

namespace NinjaWebAPI.Controllers
{
    public class NinjasController : ApiController
    {
        private readonly DisconnectedRepository _repo = new DisconnectedRepository();
        private NinjaContext db = new NinjaContext();

        // GET: api/Ninjas
        public IEnumerable<ViewListNinja> Get(string query = "", int page = 0, int pageSize = 20)
        {
            var ninjas = _repo.GetQueryableNinjasWithClan(query, page, pageSize);
            return ninjas.Select(n => new ViewListNinja
            {
                ClanName = n.Clan.ClanName,
                DateOfBirth = n.DateOfBirth,
                Id = n.Id,
                Name = n.Name,
                ServedInOniwaban = n.ServedInOniwaban
            });
        }

        public Ninja Get(int id)
        {
            return _repo.GetNinjaWithEquipmentAndClan(id);
        }
        
        // GET: api/Ninjas/5
        [ResponseType(typeof(Ninja))]
        public IHttpActionResult GetNinja(int id)
        {
            Ninja ninja = db.Ninjas.Find(id);
            if (ninja == null)
            {
                return NotFound();
            }

            return Ok(ninja);
        }

        // PUT: api/Ninjas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNinja(int id, Ninja ninja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ninja.Id)
            {
                return BadRequest();
            }

            db.Entry(ninja).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NinjaExists(id))
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

        // POST: api/Ninjas
        [ResponseType(typeof(Ninja))]
        public IHttpActionResult PostNinja(Ninja ninja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ninjas.Add(ninja);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ninja.Id }, ninja);
        }

        // DELETE: api/Ninjas/5
        [ResponseType(typeof(Ninja))]
        public IHttpActionResult DeleteNinja(int id)
        {
            Ninja ninja = db.Ninjas.Find(id);
            if (ninja == null)
            {
                return NotFound();
            }

            db.Ninjas.Remove(ninja);
            db.SaveChanges();

            return Ok(ninja);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NinjaExists(int id)
        {
            return db.Ninjas.Count(e => e.Id == id) > 0;
        }
    }
}