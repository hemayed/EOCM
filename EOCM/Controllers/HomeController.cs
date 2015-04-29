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
            return View();
        }


        public ActionResult _ClusterMap(string Govt_ID, string District_ID, string Sector_ID, string Field_ID, string Product_ID)
        {
            var clusters = (from d in db.Clusters select d).ToList();

            if (Govt_ID != "") {clusters = (from d in clusters where d.Govt_ID == Govt_ID select d).ToList();}
            if (District_ID != "0" && District_ID !="") {clusters = (from d in clusters where d.District_ID == District_ID select d).ToList();}
            if (Sector_ID != "") {clusters = (from d in clusters where d.Sector_ID == Sector_ID select d).ToList();}
            if (Field_ID != "0" && Field_ID != "") { clusters = (from d in clusters where d.Field_ID == Field_ID select d).ToList(); }
            if (Product_ID != "0" && Product_ID != "") { clusters = (from d in clusters where d.Product_ID == Product_ID select d).ToList(); }

            List<ClusterMapViewModel> listClusterMapViewModel = new List<ClusterMapViewModel>();
            var i = 1;
            foreach (Cluster cluster in clusters)
            {
              
                ClusterMapViewModel clusterMapViewModel = new ClusterMapViewModel()
                {
                    Cluster_Num=i++,
                    Cluster_Lat = cluster.Cluster_Lat,
                    Cluster_Long = cluster.Cluster_Long,
                    Cluster_ProductImage = cluster.Cluster_ProductImage,
                    Cluster_DetailPage = cluster.Cluster_DetailPage,
                    Cluster_Name = cluster.Cluster_Name,
                    Cluster_Info1 = "عدد العاملين = " + ((int)(cluster.Cluster_EmpNum)).ToString(),
                    Cluster_Info2 = "عدد الورش = " + ((int)(cluster.Cluster_ShopNum)).ToString(),
                };
            listClusterMapViewModel.Add(clusterMapViewModel);
        }

            MultipartialResult result = new MultipartialResult(this);

            var myArray = Json(listClusterMapViewModel, JsonRequestBehavior.AllowGet);

            result.AddView("_ClusterList", "ClusterListContainer", listClusterMapViewModel);
            result.AddView("_ClusterMap", "ClusterMapContainer", myArray.Data);
            
            //result.AddContent("Form", "LastClickedSpan");
           // result.AddScript(string.Format("GetMap('{0}');", myArray));

            return (result);

            //return PartialView(clusters);
        }

        public ActionResult _ClusterList(string Govt_ID, string District_ID, string Sector_ID, string Field_ID, string Product_ID)
        {

            var clusters = (from d in db.Clusters select d).ToList();

            if (Govt_ID == null)
            {
                clusters = (from d in db.Clusters select d).ToList();
            }
            else
            {
                clusters = (from d in db.Clusters where d.Govt_ID == Govt_ID select d).ToList();
            }


            return PartialView(clusters.ToList());
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public JsonResult GetDistricts(string Govt_ID)
        {
            var districts = (from d in db.Districts where d.Govt_ID == Govt_ID select d).ToList();
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

        public JsonResult GetFields(string Sector_ID)
        {
            var fields = (from d in db.Fields where d.Sector_ID == Sector_ID select d).ToList();
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
            var products = (from d in db.Products where d.Field_ID == Field_ID select d).ToList();
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