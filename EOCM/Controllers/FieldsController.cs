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
    public class FieldsController : Controller
    {
        private EOCMDB db = new EOCMDB();

        // GET: Fields
        public ActionResult Index()
        {
            var fields = db.Fields.Include(f => f.Sector);
            return View(fields.ToList());
        }

        // GET: Fields/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // GET: Fields/Create
        public ActionResult Create()
        {
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name");
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Field_ID,Field_Name,Sector_ID")] Field field)
        {
            if (ModelState.IsValid)
            {
                var id = (from d in db.Fields orderby d.Field_ID descending where (d.Sector_ID.Equals(field.Sector_ID)) select d.Field_ID).ToList();

                if (id.Count() == 0)
                {
                    field.Field_ID = field.Sector_ID + "01";
                }
                else
                {
                    string str = id[0].Substring(2, 2);

                    var fieldCode = Convert.ToInt32(str) + 1;
                    field.Field_ID = field.Sector_ID + fieldCode.ToString("00");
                }

                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", field.Sector_ID);
            return View(field);
        }

        // GET: Fields/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", field.Sector_ID);
            return View(field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Field_ID,Field_Name,Sector_ID")] Field field)
        {
            if (ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", field.Sector_ID);
            return View(field);
        }

        // GET: Fields/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
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
