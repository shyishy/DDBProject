using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using BarSystem.Data;
using BarSystem.Models;

namespace BarSystem.Controllers
{
    [Authorize]
    public class DefaultProductsController : Common
    {
        private Entities db = new Entities();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Img,Available,CreateBy,EditBy,CreateDate,EditDate")] Product product)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                //string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(file.FileName));
                //file.SaveAs(path);
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);
                string mimeType = file.ContentType;
                string base64 = Convert.ToBase64String(binData);
                product.Img = string.Format("data:{0};base64,{1}", mimeType, base64);
            }
            //string mimeType = /* Get mime type somehow (e.g. "image/png") */;
            //string base64 = Convert.ToBase64String(yourImageBytes);
            //string.Format("data:{0};base64,{1}", mimeType, base64);

            product.CreateBy = User.Identity.Name;
            product.EditBy = User.Identity.Name;
            product.CreateDate = DateTime.Now;
            product.EditDate = DateTime.Now;
            product.Available = true;

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Img,Available,CreateBy,EditBy,CreateDate,EditDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    //string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(file.FileName));
                    //file.SaveAs(path);
                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes(file.ContentLength);
                    string mimeType = file.ContentType;
                    string base64 = Convert.ToBase64String(binData);
                    if (base64 != "")
                    {
                        product.Img = string.Format("data:{0};base64,{1}", mimeType, base64);
                    }
                }

                product.EditBy = User.Identity.Name;
                product.EditDate = DateTime.Now;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
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

        
        public ActionResult jtable()
        {
            if (getRole() != "admin")
            {
                return RedirectToAction("jtable", "DefaultOrder");
            }
            return View();
        }
    }
}
