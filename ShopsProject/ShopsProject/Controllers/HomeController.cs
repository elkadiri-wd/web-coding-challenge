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
            if (Session["LoginId"] != null)
            {
                int loginId = Convert.ToInt32(Session["LoginId"]);
                double latitude = Convert.ToDouble(Session["latitude"]);
                double longitude = Convert.ToDouble(Session["longitude"]);
                var currentCoord = new GeoCoordinate(latitude, longitude);
                
                var shopsList = (from sh in shopsContext.shops
                                 where !shopsContext.UserShop.Any(us => us.shop_id == sh.id && us.login_id==loginId)
                                 select sh).ToList<shop>();

                var usersShops = (from us in shopsContext.UserShop
                                  join sh in shopsContext.shops on us.shop_id equals sh.id
                                  where us.login_id == loginId                        
                                  select us).ToList<UserShop>();

                foreach (var userShop in usersShops)
                {
                    DateTime dislikedDate = userShop.dislikedDate ?? DateTime.Now;
                    if (userShop.login_id == loginId && userShop.isPreferred == false && userShop.isDisliked == false)
                        shopsList.Add(shopsContext.shops.Where(a => a.id == userShop.shop_id).SingleOrDefault());
                    if (userShop.login_id == loginId && userShop.isDisliked == true && DateTime.Now.Subtract(dislikedDate) < new TimeSpan(2, 0, 0))
                        shopsList.Add(shopsContext.shops.Where(a => a.id == userShop.shop_id).SingleOrDefault());
                }
                
                for (int i = 0; i < shopsList.Count; i++)
                {
                    var eCoord = new GeoCoordinate(Convert.ToDouble(shopsList[i].latitude), Convert.ToDouble(shopsList[i].longitude));
                    var distance = currentCoord.GetDistanceTo(eCoord);
                    shopsList[i].distance = distance;
                }
                var orderedShopList = shopsList.OrderBy(x => x.distance).ToList();
                return View(orderedShopList);
            }

            return RedirectToAction("Login", "Account");            
        }

        public ActionResult PreferredShops()
        {
            if (Session["LoginId"] != null)
            {
                int loginId = Convert.ToInt32(Session["LoginId"]);
                ViewBag.Message = "Your Preferred Shops page";
                List<shop> preferredShops = (from sh in shopsContext.shops
                                             join us in shopsContext.UserShop on sh.id equals us.shop_id
                                             where us.isPreferred == true && us.login_id == loginId
                                             select sh).ToList<shop>();

                //var preferredShops = shopsContext.shops.Where(x => x.isPreferred == true).ToList();

                return View(preferredShops);
            }

            return RedirectToAction("Login", "Account"); 
        }

        [HttpPost]
        public string SetPreferredShop(string shopId, bool flag)
        {
            var idUser = Convert.ToInt32(Session["LoginId"]);
            var idShop = int.Parse(shopId);
            UserShop shopResult = (from us in shopsContext.UserShop
                                   where us.login_id == idUser && us.shop_id == idShop
                                   select us).SingleOrDefault();

            if (shopResult == null)
            {
                UserShop us = new UserShop();
                us.login_id = idUser;
                us.shop_id = idShop;
                us.isPreferred = flag;
                us.isDisliked = false;
                us.likedDate = DateTime.Now;

                shopsContext.UserShop.Add(us);
                shopsContext.SaveChanges();
            }
            else if (flag == true)
            {
                shopResult.isPreferred = true;
                shopsContext.SaveChanges();
            }
            else if (flag == false)
            {
                shopResult.isPreferred = false;
                shopResult.isDisliked = true;
                shopResult.dislikedDate = DateTime.Now;
                shopsContext.SaveChanges();
            }

            return "";
        }

        [HttpPost]
        public string SetPosition(string latitude, string longitude)
        {
            Session["latitude"] = latitude;
            Session["longitude"] = longitude;
            return "";
        }
    }
}
