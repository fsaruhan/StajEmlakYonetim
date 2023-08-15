using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EmlakYonetim.Models.ViewModels;
namespace EmlakYonetim.Controllers
{
    public class KontaktController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Yonetici

        public ActionResult Index()
        {
            var model = db.t_contact.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult Yeni()
        {
            var selectedContacts = db.t_contact.Where(c => c.organizasyonMu).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            var roller = db.t_roller.ToList();
            ViewBag.roller = roller;

            return View();
        }
        [HttpPost]
        public ActionResult Yeni(t_contact kontakt, HttpPostedFileBase foto, int[] secilenRoller)
        {
            if (foto != null && foto.ContentLength > 0)
            {
                string dosyaAdi = Path.GetFileName(foto.FileName);
                string hedefYol = Path.Combine(Server.MapPath("~/imgs/contact"), dosyaAdi);

                // Dosya var mı kontrolü
                if (System.IO.File.Exists(hedefYol))
                {
                    // Dosya varsa yeni bir dosya adı oluştur
                    string dosyaAdiYeni = Path.GetFileNameWithoutExtension(dosyaAdi) + "_" + DateTime.Now.Ticks + Path.GetExtension(dosyaAdi);
                    hedefYol = Path.Combine(Server.MapPath("~/imgs/contact"), dosyaAdiYeni);
                }

                foto.SaveAs(hedefYol);

                kontakt.Gorsel = "~/imgs/contact/" + Path.GetFileName(hedefYol); // Sanal dosya yolu
            }
            else
            {
                kontakt.Gorsel = "~/imgs/contact_default/dflt.jpg";
            }

            db.t_contact.Add(kontakt);
            db.SaveChanges();

            if (secilenRoller != null && secilenRoller.Length > 0)
            {
                foreach (var roleId in secilenRoller)
                {
                    var rolTablosu = new t_rolTablosu { contactID = kontakt.id, rollerID = roleId };
                    db.t_rolTablosu.Add(rolTablosu);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Kontakt");
        }


        public ActionResult Guncelle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Detay(int id)
        {
            {
                var kontakt = db.t_contact.FirstOrDefault(c => c.id == id);
                var bagliOlduguOrganizasyon = db.t_contact.FirstOrDefault(c => c.id == kontakt.organizasyonAtama)?.organizasyonAdi;
                ViewBag.bagliOlduguOrganizasyon = bagliOlduguOrganizasyon;
                var kullaniciRolleriID = db.t_rolTablosu.Where(kr => kr.contactID == id).Select(kr => kr.rollerID).ToList();
                var kullaniciRolleri = db.t_roller.Where(ra => kullaniciRolleriID.Contains(ra.id)).ToList();
                ViewBag.KullaniciRolleri = kullaniciRolleri;

                if (kontakt == null)
                {
                    return HttpNotFound();
                }

                return View(kontakt);
            }
        }
        [HttpPost]

        public ActionResult Sil(int contactId)
        {
            var kontakt = db.t_contact.Find(contactId); 
            if (kontakt == null)                   
            {
                return HttpNotFound();
            }


            var rolTablosuEntries = db.t_rolTablosu.Where(r => r.contactID == contactId);
            db.t_rolTablosu.RemoveRange(rolTablosuEntries);


            db.t_contact.Remove(kontakt);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}