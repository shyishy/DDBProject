using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BarSystem.Data;

namespace BarSystem.Controllers
{
    [Authorize]
    public class DefaultOrderController : Controller
    {
        private Entities db = new Entities();

        // GET: DefaultOrder
        public async Task<ActionResult> Index()
        {
            var c_Order = db.C_Order.Include(c => c.AspNetUser);
            return View(await c_Order.ToListAsync());
        }

        // GET: DefaultOrder/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_Order c_Order = await db.C_Order.FindAsync(id);
            if (c_Order == null)
            {
                return HttpNotFound();
            }
            return View(c_Order);
        }

        // GET: DefaultOrder/Create
        public ActionResult Create()
        {
            ViewBag.OwerId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: DefaultOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OwerId,Available,CreateBy,EditBy,CreateDate,EditDate,TableNumber,Payed")] C_Order c_Order)
        {
            c_Order.CreateBy = User.Identity.Name;
            c_Order.EditBy = User.Identity.Name;
            c_Order.CreateDate = DateTime.Now;
            c_Order.EditDate = DateTime.Now;
            c_Order.Available = true;

            if (ModelState.IsValid)
            {
                db.C_Order.Add(c_Order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OwerId = new SelectList(db.AspNetUsers, "Id", "Email", c_Order.OwerId);
            return View(c_Order);
        }

        // GET: DefaultOrder/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_Order c_Order = await db.C_Order.FindAsync(id);
            if (c_Order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwerId = new SelectList(db.AspNetUsers, "Id", "Email", c_Order.OwerId);
            return View(c_Order);
        }

        // POST: DefaultOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OwerId,Available,CreateBy,EditBy,CreateDate,EditDate,TableNumber,Payed")] C_Order c_Order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c_Order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OwerId = new SelectList(db.AspNetUsers, "Id", "Email", c_Order.OwerId);
            return View(c_Order);
        }

        // GET: DefaultOrder/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_Order c_Order = await db.C_Order.FindAsync(id);
            if (c_Order == null)
            {
                return HttpNotFound();
            }
            return View(c_Order);
        }

        // POST: DefaultOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            C_Order c_Order = await db.C_Order.FindAsync(id);
            db.C_Order.Remove(c_Order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult jtable()
        {
            return View();
        }
    }
}
