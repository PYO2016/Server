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

        /*
        // GET: api/ParsedTables
        public IQueryable<ParsedTable> GetParsedTables()
        {
            return db.ParsedTables;
        }
        */

        // GET: api/ParsedTables?pk=asdfasf
        public List<ParsedTableInner> GetParsedTables(String pk)
        {
            IQueryable<ParsedTable> tables = db.ParsedTables;
            List<ParsedTableInner> returnVal = new List<ParsedTableInner>();
            foreach (ParsedTable table in tables)
            {
                if (table.fk_User != pk)
                    continue;

                returnVal.Add(new ParsedTableInner(table.pk, table.fk_User, table.isProccessed, table.filename, table.result, table.time));
            }
            return returnVal;
        }

        // GET: api/ParsedTables?pk=asdfasf/5
        [ResponseType(typeof(ParsedTable))]
        //[TODO] 해당 id(primary key)에 해당되는 값 반환
        public IHttpActionResult GetParsedTable(String pk, int id)
        {
            ParsedTable parsedTable = db.ParsedTables.Find(id);
            if (parsedTable == null)
            {
                return NotFound();
            }
            else if (pk != parsedTable.fk_User)
            {
                return BadRequest();
            }

            return Ok(parsedTable);
        }

        /*
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
*/
        /*
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
        */

        // DELETE: api/ParsedTables?pk=asdfasf/5
        [ResponseType(typeof(ParsedTable))]
        public IHttpActionResult DeleteParsedTable(String pk, int id)
        {
            ParsedTable parsedTable = db.ParsedTables.Find(id);
            if (parsedTable == null)
            {
                return NotFound();
            }
            else if (parsedTable.isProccessed == false)
            {
                return BadRequest();
            }
            else if (parsedTable.fk_User != pk)
            {
                return BadRequest();
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