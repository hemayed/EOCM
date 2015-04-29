using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EOCM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "_ClusterList",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "_ClusterList", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "_ClusterMap",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "_ClusterMap", id = UrlParameter.Optional }
         );

      //      routes.MapRoute(
      //      name: "_GetDistricts",
      //      url: "{controller}/{action}/{id}",
      //      defaults: new { controller = "Home", action = "GetDistricts", id = UrlParameter.Optional }
      //  );

      //      routes.MapRoute(
      //    name: "_GetVillages",
      //    url: "{controller}/{action}/{id}",
      //    defaults: new { controller = "Home", action = "GetVillages", id = UrlParameter.Optional }
      //);

      //      routes.MapRoute(
      //      name: "_GetFields",
      //      url: "{controller}/{action}/{id}",
      //      defaults: new { controller = "Home", action = "GetFields", id = UrlParameter.Optional }
      //  );
      //      routes.MapRoute(
      //     name: "_GetProducts",
      //     url: "{controller}/{action}/{id}",
      //     defaults: new { controller = "Home", action = "GetProducts", id = UrlParameter.Optional }
      // );
        }
    }
}
