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
    public class VillagesController : Controller
    {
        private EOCMDB db = new EOCMDB();

        // GET: Villages
        public ActionResult Index()
        {
            var villages = db.Villages.Include(v => v.District).Include(v => v.Governorate);
            return View(villages.ToList());
        }

        // GET: Villages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // GET: Villages/Create
        public ActionResult Create()
        {
            
            
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name");
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name");
            
            return View();
        }

        // POST: Villages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Village_ID,Village_Name,Govt_ID,District_ID")] Village village)
        {
            if (ModelState.IsValid)
            {
                var id = (from d in db.Villages orderby d.Village_ID descending where (d.District_ID.Equals(village.District_ID)) select d.Village_ID).ToList();

                if (id.Count() == 0)
                {
                    village.Village_ID = village.District_ID + "01";
                }
                else
                {
                    string str = id[0].Substring(4, 2);

                    var villageCode = Convert.ToInt32(str) + 1;
                    village.Village_ID = village.District_ID + villageCode.ToString("00");
                }

                db.Villages.Add(village);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", village.District_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", village.Govt_ID);
            return View(village);
        }

        // GET: Villages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", village.District_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", village.Govt_ID);
            return View(village);
        }

        // POST: Villages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Village_ID,Village_Name,Govt_ID,District_ID")] Village village)
        {
            if (ModelState.IsValid)
            {
                db.Entry(village).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", village.District_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", village.Govt_ID);
            return View(village);
        }

        // GET: Villages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Village village = db.Villages.Find(id);
            db.Villages.Remove(village);
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
