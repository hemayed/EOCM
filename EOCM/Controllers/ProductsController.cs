using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EOCM.Models;

namespace EOCM.Controllers
{
    public class ProductsController : Controller
    {
        private EOCMDB db = new EOCMDB();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Field).Include(p => p.Sector);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
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
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name");
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_ID,Product_Name,Sector_ID,Field_ID")] Product product)
        {
            if (ModelState.IsValid)
            {
                var id = (from d in db.Products orderby d.Product_ID descending where (d.Field_ID.Equals(product.Field_ID)) select d.Product_ID).ToList();

                if (id.Count() == 0)
                {
                    product.Product_ID = product.Product_ID + "01";
                }
                else
                {
                    string str = id[0].Substring(4, 2);

                    var productCode = Convert.ToInt32(str) + 1;
                    product.Product_ID = product.Field_ID + productCode.ToString("00");
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", product.Field_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", product.Sector_ID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", product.Field_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", product.Sector_ID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_ID,Product_Name,Sector_ID,Field_ID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", product.Field_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", product.Sector_ID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
    }
}
