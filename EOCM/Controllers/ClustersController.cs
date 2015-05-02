using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EOCM.Models;
using System.Web.UI.WebControls;

namespace EOCM.Controllers
{
    public class ClustersController : Controller
    {
        private EOCMDB db = new EOCMDB();

        // GET: Clusters
        public ActionResult Index()
        {
            var clusters = db.Clusters.Include(c => c.District).Include(c => c.Field).Include(c => c.Governorate).Include(c => c.Product).Include(c => c.Sector).Include(c => c.Village);
            return View(clusters.ToList());
        }

        // GET: Clusters/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cluster cluster = db.Clusters.Find(id);
            if (cluster == null)
            {
                return HttpNotFound();
            }
            return View(cluster);
        }

       

        // GET: Clusters/Create
        public ActionResult Create()
        {
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name");
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name");
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name");
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name");
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name");
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name");
            return View();
        }

        // POST: Clusters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cluster_Name,Govt_ID,District_ID,Village_ID,Sector_ID,Field_ID,Product_ID,Cluster_Lat,Cluster_Long,Cluster_EmpNum,Cluster_ShopNum,Cluster_ProductImage,Cluster_ProcessImage,Cluster_DetailPage")] Cluster cluster)
        {
            var id = (from d in db.Clusters orderby d.Cluster_ID descending where (d.Village_ID.Equals(cluster.Village_ID) && d.Product_ID.Equals(cluster.Product_ID)) select d.Cluster_ID).ToList();
            if (id.Count() == 0)
            {
                cluster.Cluster_ID = cluster.Village_ID + cluster.Product_ID + "01";
            }
            else
            {
                string str = id[0].Substring(12, 2);

                var clusterCode = Convert.ToInt32(str)+1;
                cluster.Cluster_ID = cluster.Village_ID + cluster.Product_ID + clusterCode.ToString("00");
            }

            WebRequest wr = WebRequest.Create(cluster.Cluster_ProductImage);
            try
            {
                 System.Net.WebClient wc = new System.Net.WebClient();
                 string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProductImage);
                 string imgFile = Server.MapPath("~/Images/")+cluster.Cluster_ID+imgExt;
                 wc.DownloadFile(cluster.Cluster_ProductImage, imgFile);
                 cluster.Cluster_ProductImage = "Images/"+cluster.Cluster_ID+imgExt;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }
           

            if (ModelState.IsValid)
            {
                db.Clusters.Add(cluster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", cluster.District_ID);
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", cluster.Field_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", cluster.Govt_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", cluster.Product_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", cluster.Sector_ID);
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name", cluster.Village_ID);
            return View(cluster);
        }

        // GET: Clusters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cluster cluster = db.Clusters.Find(id);
            if (cluster == null)
            {
                return HttpNotFound();
            }
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", cluster.District_ID);
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", cluster.Field_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", cluster.Govt_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", cluster.Product_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", cluster.Sector_ID);
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name", cluster.Village_ID);
            return View(cluster);
        }

        // POST: Clusters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cluster_ID,Cluster_Name,Govt_ID,District_ID,Village_ID,Sector_ID,Field_ID,Product_ID,Cluster_Lat,Cluster_Long,Cluster_EmpNum,Cluster_ShopNum,Cluster_ProductImage,Cluster_ProcessImage,Cluster_DetailPage")] Cluster cluster)
        {

            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProductImage);
                string imgFile = Server.MapPath("~/Images/") + cluster.Cluster_ID + imgExt;
                wc.DownloadFile(cluster.Cluster_ProductImage, imgFile);
                cluster.Cluster_ProductImage = "Images/" + cluster.Cluster_ID + imgExt;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", cluster.District_ID);
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", cluster.Field_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", cluster.Govt_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", cluster.Product_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", cluster.Sector_ID);
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name", cluster.Village_ID);
            return View(cluster);
        }

        // GET: Clusters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cluster cluster = db.Clusters.Find(id);
            if (cluster == null)
            {
                return HttpNotFound();
            }
            return View(cluster);
        }

        // POST: Clusters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cluster cluster = db.Clusters.Find(id);
            db.Clusters.Remove(cluster);
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
