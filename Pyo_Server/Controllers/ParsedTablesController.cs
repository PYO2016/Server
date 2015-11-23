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
using Pyo_Server.Models;

namespace Pyo_Server.Controllers
{
    public class ParsedTablesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ParsedTables
        public IQueryable<ParsedTable> GetParsedTables()
        {
            return db.ParsedTables;
        }

        // GET: api/ParsedTables/5
        [ResponseType(typeof(ParsedTable))]
        public IHttpActionResult GetParsedTable(int id)
        {
            ParsedTable parsedTable = db.ParsedTables.Find(id);
            if (parsedTable == null)
            {
                return NotFound();
            }

            return Ok(parsedTable);
        }

        // PUT: api/ParsedTables/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParsedTable(int id, ParsedTable parsedTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parsedTable.pk)
            {
                return BadRequest();
            }

            db.Entry(parsedTable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParsedTableExists(id))
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

        // POST: api/ParsedTables
        [ResponseType(typeof(ParsedTable))]
        public IHttpActionResult PostParsedTable(ParsedTable parsedTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParsedTables.Add(parsedTable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = parsedTable.pk }, parsedTable);
        }

        // DELETE: api/ParsedTables/5
        [ResponseType(typeof(ParsedTable))]
        public IHttpActionResult DeleteParsedTable(int id)
        {
            ParsedTable parsedTable = db.ParsedTables.Find(id);
            if (parsedTable == null)
            {
                return NotFound();
            }

            db.ParsedTables.Remove(parsedTable);
            db.SaveChanges();

            return Ok(parsedTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParsedTableExists(int id)
        {
            return db.ParsedTables.Count(e => e.pk == id) > 0;
        }
    }
}