using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EmlakYonetim.Models.Entity;
using EmlakYonetim.Models.ViewModels;
namespace EmlakYonetim.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Security
       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KullaniciEkleView kullanici)
        {
            var kullaniciMi = db.t_kullanici.FirstOrDefault(x => x.email == kullanici.Email && x.sifre == kullanici.Sifre);
            if (kullaniciMi != null)
            {
                var contact = db.t_contact.FirstOrDefault(x => x.id == kullaniciMi.kayit_id);
                var rol = db.t_roller.FirstOrDefault(x => x.id == contact.yetkiGrubuID);
                if (contact.organizasyonMu)
                {
                    FormsAuthentication.SetAuthCookie(contact.id.ToString(), false);
                   
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(contact.id.ToString(), false);
                    
                }

                return RedirectToAction("Index", "Kontakt");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz E-mail ya da Şifre!";
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ViewBag.Mesaj = "Başarıyla Çıkış Yapıldı!";
            return View("Login");
        }
    }
}
