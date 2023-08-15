using EmlakYonetim.Models.Entity;
using EmlakYonetim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmlakYonetim.Controllers
{
    public class TestController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Test
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Kaydet(Test model)
        {
            var Latitude = model.Latitude;
            var Longitude = model.Longitude;
            return RedirectToAction("Goruntule", new { Lat = Latitude, Lng = Longitude });

        }
        public ActionResult Goruntule(string Lat, string Lng)
        {
            
            ViewBag.Latitude = Lat;
            ViewBag.Longitude = Lng;

            return View();
        }
    }
}