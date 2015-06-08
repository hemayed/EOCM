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
    public class DistrictsController : Controller
    {
        private EOCMDB db = new EOCMDB();

        // GET: Districts
        public ActionResult Index()
        {
            var districts = db.Districts.Include(d => d.Governorate);
            return View(districts.ToList());
        }

        // GET: Districts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name");
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "District_ID,District_Name,Govt_ID")] District district)
        {
            if (ModelState.IsValid)
            {
                var id = (from d in db.Districts orderby d.District_ID descending where (d.Govt_ID.Equals(district.Govt_ID)) select d.District_ID).ToList();

                if (id.Count() == 0)
                {
                    district.District_ID = district.Govt_ID + "01";
                }
                else
                {
                    string str = id[0].Substring(2, 2);

                    var districtCode = Convert.ToInt32(str) + 1;
                    district.District_ID = district.Govt_ID + districtCode.ToString("00");
                }

                db.Districts.Add(district);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", district.Govt_ID);
            return View(district);
        }

        // GET: Districts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", district.Govt_ID);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "District_ID,District_Name,Govt_ID")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", district.Govt_ID);
            return View(district);
        }

        // GET: Districts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
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
