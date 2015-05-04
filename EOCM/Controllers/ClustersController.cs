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
using System.IO;

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

        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
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

            if (cluster.Cluster_ProductImage != null)
            {
                 string sourceName = Path.GetFileName(Request.Files[0].FileName);
                 string imgExt = System.IO.Path.GetExtension(sourceName);
                 //string destName = AppDomain.CurrentDomain.BaseDirectory + "ProductImage/"+cluster.Cluster_ID + imgExt;
                 string destName = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;
                 
                Request.Files[0].SaveAs(destName);
                cluster.Cluster_ProductImage = "ProductImages/" + cluster.Cluster_ID + imgExt;
                }

               

                //Uri uri = new Uri(cluster.Cluster_ProductImage);
                //if (uri.IsFile)
                //{
                //  //  string filename = System.IO.Path.GetFileName(uri.LocalPath);

                //    Byte[] bytes = ReadImageFile(uri.AbsoluteUri);
                //    Console.WriteLine("Filename = " + uri.LocalPath);
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                //    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                //    string imgExt = System.IO.Path.GetExtension(uri.LocalPath);
                //    string imgFile = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;
                //    img.Save(imgFile);
                //    cluster.Cluster_ProductImage = "ProductImages/" + cluster.Cluster_ID + imgExt;
                //}
            

            if (cluster.Cluster_ProcessImage != null)
            {
                Byte[] bytes = ReadImageFile(cluster.Cluster_ProcessImage);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProcessImage);
                string imgFile = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;
                img.Save(imgFile);
                cluster.Cluster_ProcessImage = "ProcessImages/" + cluster.Cluster_ID + imgExt;
            }

            //WebRequest wr = WebRequest.Create(cluster.Cluster_ProductImage);
            //try
            //{
            //     System.Net.WebClient wc = new System.Net.WebClient();
            //     string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProductImage);
            //     string imgFile = Server.MapPath("~/ProductImages/")+cluster.Cluster_ID+imgExt;
            //     wc.DownloadFile(cluster.Cluster_ProductImage, imgFile);
            //     cluster.Cluster_ProductImage = "ProductImages/" + cluster.Cluster_ID + imgExt;
            //}
            //catch (InvalidCastException e)
            //{
            //    Console.WriteLine(e);
            //}

            //try
            //{
            //    System.Net.WebClient wc = new System.Net.WebClient();
            //    string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProcessImage);
            //    string imgFile = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;
            //    wc.DownloadFile(cluster.Cluster_ProcessImage, imgFile);
            //    cluster.Cluster_ProcessImage = "ProcessImages/" + cluster.Cluster_ID + imgExt;
            //}
            //catch (InvalidCastException e)
            //{
            //    Console.WriteLine(e);
            //}

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

            if (cluster.Cluster_ProductImage != null)
            {
                Byte[] bytes = ReadImageFile(cluster.Cluster_ProductImage);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProductImage);
                string imgFile = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;
                img.Save(imgFile);
                cluster.Cluster_ProductImage = "ProductImages/" + cluster.Cluster_ID + imgExt;
            }
            else
            {
                Cluster myCluster = db.Clusters.Find(cluster.Cluster_ID);
                cluster.Cluster_ProductImage = myCluster.Cluster_ProductImage;
            }

            if (cluster.Cluster_ProcessImage != null)
            {
                Byte[] bytes = ReadImageFile(cluster.Cluster_ProcessImage);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProcessImage);
                string imgFile = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;
                img.Save(imgFile);
                cluster.Cluster_ProcessImage = "ProcessImages/" + cluster.Cluster_ID + imgExt;
            }
            else
            {
                Cluster myCluster = db.Clusters.Find(cluster.Cluster_ID);
                cluster.Cluster_ProcessImage = myCluster.Cluster_ProcessImage;
            }

            //try
            //{
            //    System.Net.WebClient wc = new System.Net.WebClient();
            //    string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProductImage);
            //    string imgFile = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;
            //    if(cluster.Cluster_ProductImage != null)
            //    {
            //        wc.DownloadFile(cluster.Cluster_ProductImage, imgFile); 
            //        cluster.Cluster_ProductImage = "ProductImages/" + cluster.Cluster_ID + imgExt;
            //    }
            //    else
            //    {
            //        Cluster myCluster = db.Clusters.Find(cluster.Cluster_ID);
            //        cluster.Cluster_ProductImage = myCluster.Cluster_ProductImage;
            //    }
               
            //}
            //catch (InvalidCastException e)
            //{
            //    Console.WriteLine(e);
            //}

            //try
            //{
            //    System.Net.WebClient wc = new System.Net.WebClient();
            //    string imgExt = System.IO.Path.GetExtension(cluster.Cluster_ProcessImage);
            //    string imgFile = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;
            //    if (cluster.Cluster_ProcessImage != null)
            //    {
            //        wc.DownloadFile(cluster.Cluster_ProcessImage, imgFile);
            //        cluster.Cluster_ProcessImage = "ProcessImages/" + cluster.Cluster_ID + imgExt;
            //    }
            //    else
            //    {
            //        Cluster myCluster = db.Clusters.Find(cluster.Cluster_ID);
            //        cluster.Cluster_ProcessImage = myCluster.Cluster_ProcessImage;
            //    }
                
            //}
            //catch (InvalidCastException e)
            //{
            //    Console.WriteLine(e);
            //}

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
