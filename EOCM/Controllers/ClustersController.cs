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

            ViewBag.ClusterNature_ID = new SelectList(db.ClusterNature, "ClusterNature_ID", "ClusterNature_Name");
            ViewBag.ClusterType_ID = new SelectList(db.ClusterType, "ClusterType_ID", "ClusterType_Name");
            ViewBag.MarketType_ID = new SelectList(db.MarketType, "MarketType_ID", "MarketType_Name");
            ViewBag.ProductSeason_ID = new SelectList(db.ProductSeason, "ProductSeason_ID", "ProductSeason_Name");

            ViewBag.ExportFlag_ID = new SelectList(db.ExportFlag, "ExportFlag_ID", "ExportFlag_Name");
            ViewBag.IncomeLevel_ID = new SelectList(db.IncomeLevel, "IncomeLevel_ID", "IncomeLevel_Name");
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
        public ActionResult Create([Bind(Include = "Cluster_Name,Govt_ID,District_ID,Village_ID,Sector_ID,Field_ID,Product_ID,Cluster_Lat,Cluster_Long,Cluster_EmpNumMin,Cluster_EmpFemale,Cluster_ShopNumMin,Cluster_ProductImage,Cluster_ProcessImage,Cluster_DetailPage1,Cluster_DetailPage2,Cluster_DetailPage3,ClusterNature_ID,ClusterType_ID,NonOfficalProjects,OfficalProjects,Products,MoreProducts,Address,Challenges,MoreChallenges,SupportingOrg,Cluster_StudyFile1,Cluster_StudyFile2,CompanyPercent1,CompanyPercent2,CompanyPercent3,CompanyPercent4,MarketType_ID,ProductSeason_ID,ProductSeasonDetail,ExportFlag_ID,ExportVolume,Income,IncomeLevel_ID,AdditionalInfo")] Cluster cluster)
        {
            var clustername = (from d in db.Clusters where (d.Cluster_Lat.Equals(cluster.Cluster_Lat) && d.Cluster_Long.Equals(cluster.Cluster_Long)) select d.Cluster_Name).ToList();
            
            if (clustername.Count()==0)
            {
                if (ModelState.IsValid)
                {
            
                    var id = (from d in db.Clusters orderby d.Cluster_ID descending where (d.Govt_ID.Equals(cluster.Govt_ID) && d.Product_ID.Equals(cluster.Product_ID)) select d.Cluster_ID).ToList();

                    if (id.Count() == 0)
                    {
                        cluster.Cluster_ID = cluster.Govt_ID + cluster.Product_ID + "01";
                    }
                    else
                    {
                        string str = id[0].Substring(8, 2);

                        var clusterCode = Convert.ToInt32(str) + 1;
                        cluster.Cluster_ID = cluster.Govt_ID + cluster.Product_ID + clusterCode.ToString("00");
                    }

                    string sourceName;
                    string imgExt;
                    string destName;
                                        
                    if (cluster.Cluster_StudyFile1 != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[0].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                        //destName = Server.MapPath("~/StudyFiles/") + cluster.Cluster_ID + imgExt;
                        destName = Server.MapPath("~/StudyFiles/") + sourceName ;
                        Request.Files[0].SaveAs(destName);
                        cluster.Cluster_StudyFile1 =  sourceName;

                    }   

                    if (cluster.Cluster_StudyFile2 != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[1].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                        //destName = Server.MapPath("~/StudyFiles/") + cluster.Cluster_ID + "_2" + imgExt;
                        destName = Server.MapPath("~/StudyFiles/") + sourceName;
                        Request.Files[1].SaveAs(destName);
                        cluster.Cluster_StudyFile2 =  sourceName;

                    }
                    if (cluster.Cluster_ProductImage != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[2].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                        //string destName = AppDomain.CurrentDomain.BaseDirectory + "ProductImage/"+cluster.Cluster_ID + imgExt;
                        destName = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;

                        Request.Files[2].SaveAs(destName);
                        cluster.Cluster_ProductImage = cluster.Cluster_ID + imgExt;
                    }


                    if (cluster.Cluster_ProcessImage != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[3].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                        //string destName = AppDomain.CurrentDomain.BaseDirectory + "ProductImage/"+cluster.Cluster_ID + imgExt;
                        destName = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;

                        Request.Files[3].SaveAs(destName);
                        cluster.Cluster_ProcessImage =  cluster.Cluster_ID + imgExt;
                    }

                   

                    db.Clusters.Add(cluster);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ValidationSummary = clustername[0] + " له نفس خط الطول والعرض ";
            } 
               
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", cluster.District_ID);
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", cluster.Field_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", cluster.Govt_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", cluster.Product_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", cluster.Sector_ID);
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name", cluster.Village_ID);

            ViewBag.ClusterNature_ID = new SelectList(db.ClusterNature, "ClusterNature_ID", "ClusterNature_Name",cluster.ClusterNature_ID);
            ViewBag.ClusterType_ID = new SelectList(db.ClusterType, "ClusterType_ID", "ClusterType_Name",cluster.ClusterNature_ID );
            ViewBag.MarketType_ID = new SelectList(db.MarketType, "MarketType_ID", "MarketType_Name",cluster.MarketType_ID);
            ViewBag.ProductSeason_ID = new SelectList(db.ProductSeason, "ProductSeason_ID", "ProductSeason_Name",cluster.ProductSeason_ID );
            ViewBag.ExportFlag_ID = new SelectList(db.ExportFlag, "ExportFlag_ID", "ExportFlag_Name", cluster.ExportFlag_ID);
            ViewBag.IncomeLevel_ID = new SelectList(db.IncomeLevel, "IncomeLevel_ID", "IncomeLevel_Name", cluster.IncomeLevel_ID);

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
            ViewBag.ClusterNature_ID = new SelectList(db.ClusterNature, "ClusterNature_ID", "ClusterNature_Name", cluster.ClusterNature_ID);
            ViewBag.ClusterType_ID = new SelectList(db.ClusterType, "ClusterType_ID", "ClusterType_Name", cluster.ClusterType_ID);
            ViewBag.MarketType_ID = new SelectList(db.MarketType, "MarketType_ID", "MarketType_Name", cluster.MarketType_ID);
            ViewBag.ProductSeason_ID = new SelectList(db.ProductSeason, "ProductSeason_ID", "ProductSeason_Name", cluster.ProductSeason_ID);
            ViewBag.ExportFlag_ID = new SelectList(db.ExportFlag, "ExportFlag_ID", "ExportFlag_Name", cluster.ExportFlag_ID);
            ViewBag.IncomeLevel_ID = new SelectList(db.IncomeLevel, "IncomeLevel_ID", "IncomeLevel_Name", cluster.IncomeLevel_ID);

            return View(cluster);
        }

        // POST: Clusters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Cluster_ID,Cluster_Name,Govt_ID,District_ID,Village_ID,Sector_ID,Field_ID,Product_ID,Cluster_Lat,Cluster_Long,Cluster_EmpNumMin,Cluster_EmpFemale,Cluster_ShopNumMin,Cluster_ProductImage,Cluster_ProcessImage,Cluster_DetailPage1,Cluster_DetailPage2,Cluster_DetailPage3,ClusterNature_ID,ClusterType_ID,NonOfficalProjects,OfficalProjects,Products,MoreProducts,Address,Challenges,MoreChallenges,SupportingOrg,Cluster_StudyFile1,Cluster_StudyFile2,CompanyPercent1,CompanyPercent2,CompanyPercent3,CompanyPercent4,MarketType_ID,ProductSeason_ID,ProductSeasonDetail,ExportFlag_ID,ExportVolume,Income,IncomeLevel_ID,AdditionalInfo")] Cluster cluster)
        {

            string sourceName;
            string imgExt;
            string destName;

            var clustername = (from d in db.Clusters where (d.Cluster_Lat.Equals(cluster.Cluster_Lat) && d.Cluster_Long.Equals(cluster.Cluster_Long) && d.Cluster_ID!=cluster.Cluster_ID) select d.Cluster_Name).ToList();
            
                if (clustername.Count()==0)
                {
                if (ModelState.IsValid)
                {
                    Cluster myCluster = db.Clusters.Find(cluster.Cluster_ID);
                    myCluster.Cluster_Lat = cluster.Cluster_Lat;
                    myCluster.Cluster_Long = cluster.Cluster_Long;
                    myCluster.Cluster_Name = cluster.Cluster_Name;
                    myCluster.Cluster_ShopNumMin = cluster.Cluster_ShopNumMin;
                    myCluster.Cluster_EmpNumMin = cluster.Cluster_EmpNumMin;
                  
                   
                    myCluster.Cluster_EmpFemale  = cluster.Cluster_EmpFemale;

                    myCluster.Cluster_DetailPage1 = cluster.Cluster_DetailPage1;
                    myCluster.Cluster_DetailPage2 = cluster.Cluster_DetailPage2;
                    myCluster.Cluster_DetailPage3 = cluster.Cluster_DetailPage3;

                    myCluster.Govt_ID = cluster.Govt_ID;
                    myCluster.District_ID = cluster.District_ID;
                    myCluster.Village_ID = cluster.Village_ID;
                    myCluster.Sector_ID = cluster.Sector_ID;
                    myCluster.Field_ID = cluster.Field_ID;
                    myCluster.Product_ID = cluster.Product_ID;
                    myCluster.Address = cluster.Address;

                    myCluster.Challenges = cluster.Challenges;
                    myCluster.MoreChallenges = cluster.MoreChallenges;
                  
                    myCluster.NonOfficalProjects = cluster.NonOfficalProjects;
                    myCluster.OfficalProjects = cluster.OfficalProjects;

                    myCluster.Products = cluster.Products;
                    myCluster.MoreProducts = cluster.MoreProducts;

                    myCluster.SupportingOrg = cluster.SupportingOrg;
                    
                
                    myCluster.CompanyPercent1 = cluster.CompanyPercent1;
                    myCluster.CompanyPercent2 = cluster.CompanyPercent2;
                    myCluster.CompanyPercent3 = cluster.CompanyPercent3;
                    myCluster.CompanyPercent4 = cluster.CompanyPercent4;

                    myCluster.ClusterNature_ID = cluster.ClusterNature_ID;
                    myCluster.ClusterType_ID = cluster.ClusterType_ID;
                    myCluster.ExportFlag_ID = cluster.ExportFlag_ID;
                    myCluster.ExportVolume = cluster.ExportVolume;
                    myCluster.MarketType_ID = cluster.MarketType_ID;
                    myCluster.ProductSeason_ID = cluster.ProductSeason_ID;
                    myCluster.ProductSeasonDetail = cluster.ProductSeasonDetail;

                    myCluster.IncomeLevel_ID = cluster.IncomeLevel_ID;
                    //myCluster.Income = cluster.Income;

                    myCluster.AdditionalInfo = cluster.AdditionalInfo;

                    if (cluster.Cluster_StudyFile1 != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[0].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                       //destName = Server.MapPath("~/StudyFiles/") + cluster.Cluster_ID + "_1" + imgExt;
                        destName = Server.MapPath("~/StudyFiles/") + sourceName;
                        Request.Files[0].SaveAs(destName);
                        myCluster.Cluster_StudyFile1 = sourceName;

                    }

                    if (cluster.Cluster_StudyFile2 != null)
                    {
                        sourceName = Path.GetFileName(Request.Files[1].FileName);
                        imgExt = System.IO.Path.GetExtension(sourceName);
                        destName = Server.MapPath("~/StudyFiles/") + sourceName;

                        Request.Files[1].SaveAs(destName);
                        myCluster.Cluster_StudyFile2 = sourceName;

                    }

                if (cluster.Cluster_ProductImage != null)
                {
                     sourceName = Path.GetFileName(Request.Files[2].FileName);
                     imgExt = System.IO.Path.GetExtension(sourceName);
                    //string destName = AppDomain.CurrentDomain.BaseDirectory + "ProductImage/"+cluster.Cluster_ID + imgExt;
                     destName = Server.MapPath("~/ProductImages/") + cluster.Cluster_ID + imgExt;

                    Request.Files[2].SaveAs(destName);
                    myCluster.Cluster_ProductImage = cluster.Cluster_ID + imgExt;
             
                }
           

                if (cluster.Cluster_ProcessImage != null)
                {
                     sourceName = Path.GetFileName(Request.Files[3].FileName);
                     imgExt = System.IO.Path.GetExtension(sourceName);
                    //string destName = AppDomain.CurrentDomain.BaseDirectory + "ProductImage/"+cluster.Cluster_ID + imgExt;
                     destName = Server.MapPath("~/ProcessImages/") + cluster.Cluster_ID + imgExt;

                    Request.Files[3].SaveAs(destName);
                    myCluster.Cluster_ProcessImage = cluster.Cluster_ID + imgExt;
                }
           

                db.Entry(myCluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ValidationSummary = clustername[0] + " له نفس خط الطول والعرض ";
            } 

            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name", cluster.District_ID);
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name", cluster.Field_ID);
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name", cluster.Govt_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", cluster.Product_ID);
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name", cluster.Sector_ID);
            ViewBag.Village_ID = new SelectList(db.Villages, "Village_ID", "Village_Name", cluster.Village_ID);
            ViewBag.ClusterNature_ID = new SelectList(db.ClusterNature, "ClusterNature_ID", "ClusterNature_Name", cluster.ClusterNature_ID);
            ViewBag.ClusterType_ID = new SelectList(db.ClusterType, "ClusterType_ID", "ClusterType_Name", cluster.ClusterType_ID);
            ViewBag.MarketType_ID = new SelectList(db.MarketType, "MarketType_ID", "MarketType_Name", cluster.MarketType_ID);
            ViewBag.ProductSeason_ID = new SelectList(db.ProductSeason, "ProductSeason_ID", "ProductSeason_Name", cluster.ProductSeason_ID);
            ViewBag.ExportFlag_ID = new SelectList(db.ExportFlag, "ExportFlag_ID", "ExportFlag_Name", cluster.ExportFlag_ID);
            ViewBag.IncomeLevel_ID = new SelectList(db.IncomeLevel, "IncomeLevel_ID", "IncomeLevel_Name", cluster.IncomeLevel_ID);
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
