using System.Web.Mvc;
using System.Web.Routing;

namespace LinkedInDemo
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
                name: "zoomdemo",
                url: "zoomdemo",
                defaults: new { controller = "Home", action = "zoomdemo", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "bigmarkerdemo",
                url: "bigmarkerdemo",
                defaults: new { controller = "Home", action = "bigmarkerdemo", id = UrlParameter.Optional }
            );
        }
    }
}
