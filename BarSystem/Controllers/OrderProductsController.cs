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

namespace BarSystem.Controllers
{
    public class OrderProductsController : ApiController
    {
        private Entities db = new Entities();

        //// GET: api/OrderProducts
        //public IQueryable<OrderProduct> GetOrderProducts()
        //{
        //    return db.OrderProducts;
        //}

        // GET: api/OrderProducts/5
        public IQueryable<SP_OrderProducts_Result> GetOrderProduct(int id)
        {
            return db.SP_OrderProducts_By_OrderId(id).AsQueryable();
        }

        //// GET: api/OrderProducts/5
        //[ResponseType(typeof(OrderProduct))]
        //public async Task<IHttpActionResult> GetOrderProduct(int id)
        //{
        //    OrderProduct orderProduct = await db.OrderProducts.FindAsync(id);
        //    if (orderProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(orderProduct);
        //}

        // PUT: api/OrderProducts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderProduct(int id, OrderProduct orderProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderProduct.Id)
            {
                return BadRequest();
            }

            db.Entry(orderProduct).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderProductExists(id))
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

        // POST: api/OrderProducts
        [ResponseType(typeof(OrderProduct))]
        public async Task<IHttpActionResult> PostOrderProduct(OrderProduct orderProduct)
        {
            orderProduct.Available = true;
            orderProduct.CreateBy = User.Identity.Name;
            orderProduct.EditBy = User.Identity.Name;
            orderProduct.CreateDate = DateTime.Now;
            orderProduct.EditDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderProducts.Add(orderProduct);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderProduct.Id }, orderProduct);
        }

        // DELETE: api/OrderProducts/5
        [ResponseType(typeof(OrderProduct))]
        public async Task<IHttpActionResult> DeleteOrderProduct(int id)
        {
            OrderProduct orderProduct = await db.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            db.OrderProducts.Remove(orderProduct);
            await db.SaveChangesAsync();

            return Ok(orderProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderProductExists(int id)
        {
            return db.OrderProducts.Count(e => e.Id == id) > 0;
        }
    }
}