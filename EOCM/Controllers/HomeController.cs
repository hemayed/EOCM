using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOCM.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.IO;
using Pulzonic.Multipartial;
using Newtonsoft.Json;


namespace EOCM.Controllers
{
    public class HomeController : Controller
    {
        private EOCMDB db = new EOCMDB();

        

        protected internal virtual CustomJsonResult CustomJson(object json = null, bool allowGet = true)
        {
            return new CustomJsonResult(json)
            {
                JsonRequestBehavior = allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet
            };
        }


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.District_ID = new SelectList(db.Districts, "District_ID", "District_Name");
            ViewBag.Field_ID = new SelectList(db.Fields, "Field_ID", "Field_Name");
            ViewBag.Govt_ID = new SelectList(db.Governorates, "Govt_ID", "Govt_Name");
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name");
            ViewBag.Sector_ID = new SelectList(db.Sectors, "Sector_ID", "Sector_Name");

            ViewBag.ProductSeason_ID = new SelectList(db.ProductSeason, "ProductSeason_ID", "ProductSeason_Name");
            ViewBag.ClusterType_ID = new SelectList(db.ClusterType, "ClusterType_ID", "ClusterType_Name");
            ViewBag.ClusterNature_ID = new SelectList(db.ClusterNature, "ClusterNature_ID", "ClusterNature_Name");
            ViewBag.IncomeLevel_ID = new SelectList(db.IncomeLevel, "IncomeLevel_ID", "IncomeLevel_Name");
            ViewBag.ExportFlag_ID = new SelectList(db.ExportFlag, "ExportFlag_ID", "ExportFlag_Name");

            return View();
        }


        public ActionResult _ClusterMap(string Govt_ID, string District_ID, string Sector_ID, string Field_ID, string Product_ID,string ClusterNature_ID, string ClusterType_ID, string IncomeLevel_ID, string ExportFlag_ID, string ProductSeason_ID)
        {
            
            ClusterMapViewModel clusterMapViewModel = new ClusterMapViewModel();
            clusterMapViewModel = GetClusters(Govt_ID, District_ID, Sector_ID, Field_ID, Product_ID,ClusterNature_ID,  ClusterType_ID,  IncomeLevel_ID,ExportFlag_ID,ProductSeason_ID);

            MultipartialResult result = new MultipartialResult(this);
                                   
            var myArray = Json(clusterMapViewModel, JsonRequestBehavior.AllowGet);

            result.AddView("_ClusterList", "ClusterListContainer", clusterMapViewModel.ClusterData);
            result.AddView("_ClusterMap", "ClusterMapContainer", myArray.Data); 

         
            return (result);

        }

        public ActionResult OtherGovtClusters(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             var clusters = (from d in db.Clusters orderby d.Cluster_Name ascending where d.Govt_ID == id select d).ToList();

             if (clusters == null)
            {
                return HttpNotFound();
            }

           
            List<ClusterShortData> listClusterShortData = new List<ClusterShortData>();

            
            foreach (Cluster c in clusters)
            {
               ClusterShortData clusterShortData = new ClusterShortData();
               clusterShortData.Cluster_ID = c.Cluster_ID;
               clusterShortData.Cluster_Name = c.Cluster_Name;
               clusterShortData.Govt_ID = c.Govt_ID;
               clusterShortData.Govt_Name = c.Governorate.Govt_Name;
               clusterShortData.Products = c.Products;
               clusterShortData.Sector_Name = c.Sector.Sector_Name;
               clusterShortData.Field_Name = c.Field.Field_Name;
               clusterShortData.Product_Name = c.Product.Product_Name;
               listClusterShortData.Add(clusterShortData);
            }

            return View("OtherGovtClusters", listClusterShortData); 

        }

        public ActionResult SimilarClusters(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clusters = (from d in db.Clusters orderby d.Cluster_Name ascending where d.Product_ID  == id select d).ToList();

            if (clusters == null)
            {
                return HttpNotFound();
            }


            List<ClusterShortData> listClusterShortData = new List<ClusterShortData>();


            foreach (Cluster c in clusters)
            {
                ClusterShortData clusterShortData = new ClusterShortData();
                clusterShortData.Cluster_ID = c.Cluster_ID;
                clusterShortData.Cluster_Name = c.Cluster_Name;
                clusterShortData.Govt_ID = c.Govt_ID;
                clusterShortData.Govt_Name = c.Governorate.Govt_Name;
                clusterShortData.Products = c.Products;
                clusterShortData.Sector_Name = c.Sector.Sector_Name;
                clusterShortData.Field_Name = c.Field.Field_Name;
                clusterShortData.Product_Name = c.Product.Product_Name;
                listClusterShortData.Add(clusterShortData);
            }

            return View("SimilarClusters", listClusterShortData);

        }

        public ActionResult ClusterDetail(string id)
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
            return View("ClusterDetail", cluster); 

        }

        
        private ClusterMapViewModel GetClusters(string Govt_ID, string District_ID, string Sector_ID, string Field_ID, string Product_ID,string ClusterNature_ID, string ClusterType_ID, string IncomeLevel_ID, string ExportFlag_ID, string ProductSeason_ID)
        {
            var clusters = (from d in db.Clusters orderby d.Govt_ID, d.Cluster_Name ascending select d).ToList();

            if (Govt_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.Govt_ID == Govt_ID select d).ToList(); }
            if (District_ID != "0" && District_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.District_ID == District_ID select d).ToList(); }
            if (Sector_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.Sector_ID == Sector_ID select d).ToList(); }
            if (Field_ID != "0" && Field_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.Field_ID == Field_ID select d).ToList(); }
            if (Product_ID != "0" && Product_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.Product_ID == Product_ID select d).ToList(); }
            if (ClusterNature_ID != null && ClusterNature_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.ClusterNature_ID == ClusterNature_ID select d).ToList(); }
            if (ClusterType_ID != null && ClusterType_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.ClusterType_ID == ClusterType_ID select d).ToList(); }
            if (IncomeLevel_ID != null && IncomeLevel_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.IncomeLevel_ID == IncomeLevel_ID select d).ToList(); }
            if (ExportFlag_ID != null && ExportFlag_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.ExportFlag_ID == ExportFlag_ID select d).ToList(); }
            if (ProductSeason_ID != null && ProductSeason_ID != "") { clusters = (from d in clusters orderby d.Govt_ID, d.Cluster_Name ascending where d.ProductSeason_ID == ProductSeason_ID select d).ToList(); }

            //List<ClusterMapViewModel> listClusterMapViewModel = new List<ClusterMapViewModel>();
            List<GovtData> listGovtData=new List<GovtData>();
            List<ClusterData> listClusterData = new List<ClusterData>();

           for (Int16 govtID=1;govtID<=27;govtID++){
                GovtData govtData = new GovtData();
                govtData.Cluster_Num = 0;
                govtData.ShopNumMin= 0;
               
            govtData.EmpNumMin = 0;
           
           govtData.ExportNum = 0;

                   Governorate govt = db.Governorates.Find(govtID.ToString("00"));
                   govtData.Govt_ID = govt.Govt_ID;
                   govtData.Govt_Name = govt.Govt_Name;
                   govtData.Govt_Lat = govt.Govt_Lat;
                   govtData.Govt_Long = govt.Govt_Long;
                  
                   listGovtData.Add(govtData);
             }

           foreach (Cluster c in clusters)
           {
               var govtIndex = Convert.ToInt16(c.Govt_ID) - 1;
               var sectorIndex = Convert.ToInt16(c.Sector_ID) - 1;

               ClusterData clusterData = new ClusterData();
               listGovtData[govtIndex].Cluster_Num++;
               listGovtData[govtIndex].ShopNumMin = listGovtData[govtIndex].ShopNumMin + c.Cluster_ShopNumMin;
              
               listGovtData[govtIndex].EmpNumMin = listGovtData[govtIndex].EmpNumMin + c.Cluster_EmpNumMin;
               
               if (c.ExportFlag_ID == "01")
                   listGovtData[govtIndex].ExportNum++;

               clusterData.Cluster_ID=c.Cluster_ID;
               clusterData.Cluster_Name = c.Cluster_Name;
               clusterData.Cluster_Lat = c.Cluster_Lat;
               clusterData.Cluster_Long = c.Cluster_Long;
               clusterData.Sector_ID = Convert.ToInt16(c.Sector_ID);
               clusterData.Govt_ID = Convert.ToInt16(c.Govt_ID);
               clusterData.Govt_Name = c.Governorate.Govt_Name;

               if (c.District != null)
                   clusterData.District_Name = c.District.District_Name;
               else
                   clusterData.District_Name = "";

               if (c.Village != null)
                   clusterData.Village_Name = c.Village.Village_Name;
               else
                   clusterData.Village_Name = "";

               clusterData.Cluster_ProcessImage = c.Cluster_ProcessImage;
               clusterData.Cluster_ProductImage = c.Cluster_ProductImage;
               clusterData.Cluster_Info1 = c.Governorate.Govt_Name;

               if (c.District!= null)
                   clusterData.Cluster_Info1 = clusterData.Cluster_Info1+ " - " + c.District.District_Name ;
               
               if (c.Village != null)
                    clusterData.Cluster_Info1 = clusterData.Cluster_Info1 + " - " + c.Village.Village_Name;


               clusterData.Cluster_Info2 = " متوسط عدد العاملين " + ":&nbsp" ;
               if (c.Cluster_EmpNumMin != 0)
                   clusterData.Cluster_Info2=clusterData.Cluster_Info2+c.Cluster_EmpNumMin ;

              


               clusterData.Cluster_Info3 = " متوسط عدد الوحدات الانتاجية " + ":&nbsp";
               if (c.Cluster_ShopNumMin != 0)
                   clusterData.Cluster_Info3=clusterData.Cluster_Info3+c.Cluster_ShopNumMin ;

              

               clusterData.Cluster_Info4 = "نسبة التصدير: ";
               if (c.ExportVolume != null)
                   clusterData.Cluster_Info4 = clusterData.Cluster_Info4+c.ExportVolume;

            clusterData.Cluster_Info5 = "المنتجات: ";
                if (c.Products != null)
                    clusterData.Cluster_Info5= clusterData.Cluster_Info5 + c.Products;
               listClusterData.Add(clusterData);
           }

            ClusterMapViewModel clusterMapViewModel=new ClusterMapViewModel();
            clusterMapViewModel.GovtData = listGovtData;
            clusterMapViewModel.ClusterData = listClusterData;

                           
            return (clusterMapViewModel);

        }

        public JsonResult GetDistricts(string Govt_ID)
        {
            var districts = (from d in db.Districts orderby d.District_Name ascending where d.Govt_ID == Govt_ID select d).ToList();
            List<SelectListItem> districtList = new List<SelectListItem>();
            foreach (District district in districts)
            {

                SelectListItem districtItem = new SelectListItem()
                {
                   Value = district.District_ID,
                   Text=district.District_Name
                };
                districtList.Add(districtItem);
            }

            return Json(new SelectList(districtList, "Value", "Text"));
        }


        public JsonResult GetVillages(string District_ID)
        {
            var villages = ((from d in db.Villages where d.District_ID == District_ID select d).ToList()).OrderBy(d=>d.Village_Name);
            List<SelectListItem> villageList = new List<SelectListItem>();
            foreach (Village village in villages)
            {

                SelectListItem villageItem = new SelectListItem()
                {
                    Value = village.Village_ID,
                    Text = village.Village_Name
                };
                villageList.Add(villageItem);
            }

            return Json(new SelectList(villageList, "Value", "Text"));
        }

        public JsonResult GetFields(string Sector_ID)
        {
            var fields = (from d in db.Fields orderby d.Field_Name ascending where d.Sector_ID == Sector_ID select d).ToList();
            List<SelectListItem> fieldList = new List<SelectListItem>();
            foreach (Field field in fields)
            {

                SelectListItem fieldItem = new SelectListItem()
                {
                    Value = field.Field_ID,
                    Text = field.Field_Name
                };
                fieldList.Add(fieldItem);
            }

            return Json(new SelectList(fieldList, "Value", "Text"));
        }

        public JsonResult GetProducts(string Field_ID)
        {
            var products = (from d in db.Products orderby d.Product_Name ascending where d.Field_ID == Field_ID select d).ToList();
            List<SelectListItem> productList = new List<SelectListItem>();
            foreach (Product product in products)
            {

                SelectListItem productItem = new SelectListItem()
                {
                    Value = product.Product_ID,
                    Text = product.Product_Name
                };
                productList.Add(productItem);
            }

            return Json(new SelectList(productList, "Value", "Text"));
        }
    }
}