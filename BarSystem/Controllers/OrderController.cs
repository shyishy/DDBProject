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
using BarSystem.Data;
using BarSystem.Models;
using Microsoft.AspNet.Identity;

namespace BarSystem.Controllers
{
    [Authorize]
    public class OrderController : APICommon
    {
        private Entities db = new Entities();

        // GET: api/Order
        public IQueryable<SP_Order_Result> GetC_Order()
        {
            if (getRole() == "admin")
            {
                return db.SP_Order().AsQueryable();
            }
            return db.SP_Order_By_OwerId(User.Identity.GetUserId()).AsQueryable();
        }

        //// PUT: api/Order/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutC_Order(int id, C_Order c_Order)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != c_Order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(c_Order).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!C_OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Order
        [ResponseType(typeof(C_Order))]
        public async Task<IHttpActionResult> PostC_Order(C_Order c_Order)
        {
            c_Order.Available = true;
            c_Order.CreateBy = User.Identity.Name;
            c_Order.EditBy = User.Identity.Name;
            c_Order.CreateDate = DateTime.Now;
            c_Order.EditDate = DateTime.Now;
            c_Order.OwerId = User.Identity.GetUserId();



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.C_Order.Add(c_Order);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (C_OrderExists(c_Order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = c_Order.Id }, c_Order);
        }

        // DELETE: api/Order/5
        [ResponseType(typeof(C_Order))]
        public async Task<IHttpActionResult> DeleteC_Order(int id)
        {
            C_Order c_Order = await db.C_Order.FindAsync(id);
            if (c_Order == null)
            {
                return NotFound();
            }

            db.C_Order.Remove(c_Order);
            await db.SaveChangesAsync();

            return Ok(c_Order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool C_OrderExists(int id)
        {
            return db.C_Order.Count(e => e.Id == id) > 0;
        }
    }
}