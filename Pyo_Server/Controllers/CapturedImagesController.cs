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
    public class CapturedImagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CapturedImages
        public IQueryable<CapturedImage> GetCapturedImages()
        {
            return db.CapturedImages;
        }

        // GET: api/CapturedImages/5
        [ResponseType(typeof(CapturedImage))]
        public IHttpActionResult GetCapturedImage(int id)
        {
            CapturedImage capturedImage = db.CapturedImages.Find(id);
            if (capturedImage == null)
            {
                return NotFound();
            }

            return Ok(capturedImage);
        }

        // PUT: api/CapturedImages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCapturedImage(int id, CapturedImage capturedImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != capturedImage.fk_ParsedTable)
            {
                return BadRequest();
            }

            db.Entry(capturedImage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CapturedImageExists(id))
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

        // POST: api/CapturedImages
        [ResponseType(typeof(CapturedImage))]
        public IHttpActionResult PostCapturedImage(CapturedImage capturedImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CapturedImages.Add(capturedImage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = capturedImage.fk_ParsedTable }, capturedImage);
        }

        // DELETE: api/CapturedImages/5
        [ResponseType(typeof(CapturedImage))]
        public IHttpActionResult DeleteCapturedImage(int id)
        {
            CapturedImage capturedImage = db.CapturedImages.Find(id);
            if (capturedImage == null)
            {
                return NotFound();
            }

            db.CapturedImages.Remove(capturedImage);
            db.SaveChanges();

            return Ok(capturedImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CapturedImageExists(int id)
        {
            return db.CapturedImages.Count(e => e.fk_ParsedTable == id) > 0;
        }
    }
}