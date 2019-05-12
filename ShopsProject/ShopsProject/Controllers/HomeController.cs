using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Device.Location;
using System.Net.NetworkInformation;
namespace ShopsProject.Controllers
{
    public class HomeController : Controller
    {
        private shopsEntities1 shopsContext = new shopsEntities1();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NearbyShops()
        {
            try
            {
                double latitude = Convert.ToDouble(Request.QueryString["latitude"]);
                double longitude = Convert.ToDouble(Request.QueryString["longitude"]);
                shop sh = new shop();
                var currentCoord = new GeoCoordinate(latitude, longitude);
                //var eCoord = new GeoCoordinate(eLatitude, eLongitude);
                //return sCoord.GetDistanceTo(eCoord);
                var shopsList = shopsContext.shops.ToList();
                for (int i = 0; i < shopsList.Count; i++ )
                {
                    var eCoord = new GeoCoordinate(Convert.ToDouble(shopsList[i].latitude), Convert.ToDouble(shopsList[i].longitude));
                    var distance = currentCoord.GetDistanceTo(eCoord);
                    shopsList[i].distance = distance;
                }
                var orderedShopList = shopsList.OrderBy(x => x.distance).ToList();
                return View(orderedShopList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PreferredShops()
        {
            ViewBag.Message = "Your Preferred Shops page";
            var preferredShops = shopsContext.shops.Where(x => x.isPreferred == true).ToList();

            return View(preferredShops);
        }
    }
}
