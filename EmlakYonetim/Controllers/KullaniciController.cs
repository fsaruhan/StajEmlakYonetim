using EmlakYonetim.Models.Entity;
using EmlakYonetim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EmlakYonetim.Controllers
{
    [Authorize(Roles =("Admin"))]
    public class KullaniciController : Controller
    {
        // GET: Kullanici

        PMSEntities db = new PMSEntities();

        //Sayfalama işlemi (Pagination)
        public ActionResult Index()
        {
            var kullanicilar = db.t_kullanici.Include("t_contact").Include("t_yetkiGrubu").ToList();
            return View(kullanicilar);
        }
        
        [HttpGet]
        public ActionResult KullaniciEkle()
        {
            var kontakt = db.t_contact.Where(x => x.yetkiGrubuID == null).ToList();
            var roller = db.t_yetkiGrubu.ToList();
            ViewBag.Roller = roller;
            return View(kontakt); 
        }
        [HttpPost]
        public ActionResult KullaniciSil(int? userId)
        {
            if (userId != null && userId != 1)
            {
                var mulkler = db.t_mulk.Where(x => x.saticiID == userId).ToList();
                foreach (var mulk in mulkler)
                {
                    mulk.saticiID = 1;
                    
                }
                db.SaveChanges();
                var kullanici = db.t_kullanici.FirstOrDefault(x => x.kayit_id == userId);
                var kontakt = db.t_contact.FirstOrDefault(x => x.id == userId);
                TempData["SilinenId"] = kullanici.kayit_id;
                if (kontakt.organizasyonMu)
                {
                    TempData["SilinenAd"] = kontakt.organizasyonAdi;
                }
                else
                {
                    TempData["SilinenAd"] = kontakt.isim;
                }
                kontakt.yetkiGrubuID = null;
                db.t_kullanici.Remove(kullanici);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult KullaniciEkle(KullaniciEkleView model)
        {
            if (model != null)
            {
                if (model.SecilenKayit != null && model.SecilenYetki != null && model.Email != null && model.Sifre != null)
                {
                    var yeniKullanici = new t_kullanici()
                    {
                        kayit_id = (int)model.SecilenKayit,
                        yetkiGrubu = (int)model.SecilenYetki,
                        email = model.Email,
                        sifre = model.Sifre,
                        tarih = DateTime.Now.Date
                   
                     };
                    db.t_kullanici.Add(yeniKullanici);
              
                    var kontaktGuncelle = db.t_contact.FirstOrDefault(x => x.id == model.SecilenKayit);
                    kontaktGuncelle.yetkiGrubuID = model.SecilenYetki;

                db.SaveChanges();
                    if (kontaktGuncelle.organizasyonMu)
                    {
                        TempData["EklenenAd"] = kontaktGuncelle.organizasyonAdi;
                    }
                    else
                    {
                        TempData["EklenenAd"] = kontaktGuncelle.isim;
                    }
                    TempData["ID"] = kontaktGuncelle.id;
                    
                return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
              
            }
            else
            {
                return HttpNotFound();
            }

            
        }
    }
}