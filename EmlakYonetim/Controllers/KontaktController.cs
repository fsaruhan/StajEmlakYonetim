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
        [Authorize(Roles = "Admin")]
        public ActionResult Yeni()
        {
            var selectedContacts = db.t_contact.Where(c => c.organizasyonMu).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            var roller = db.t_roller.ToList();
            ViewBag.roller = roller;

            return View();
        }
        [HttpPost]
        
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Guncelle(int id)
        {
            {
                var kontakt = db.t_contact.FirstOrDefault(c => c.id == id);
                var bagliOlduguOrganizasyon = db.t_contact.FirstOrDefault(c => c.id == kontakt.organizasyonAtama)?.organizasyonAdi;
                ViewBag.bagliOlduguOrganizasyon = bagliOlduguOrganizasyon;
                var kullaniciRolleriID = db.t_rolTablosu.Where(kr => kr.contactID == id).Select(kr => kr.rollerID).ToList();
                var kullaniciRolleri = db.t_roller.Where(ra => kullaniciRolleriID.Contains(ra.id)).ToList();
                ViewBag.KullaniciRolleri = kullaniciRolleri;
                ViewBag.Organizasyonlar = db.t_contact.Where(x => x.organizasyonMu == true).ToList();
                var iletisimListesi = db.t_tercihEdilenIletisim.ToList();
                var mevcutIletisimListesi = db.t_tercihEdilenIletisim.FirstOrDefault(x => x.id == kontakt.tercihEdilenIletisim);
                ViewBag.IletisimListesi = iletisimListesi;
                ViewBag.MevcutIletisimListesi = mevcutIletisimListesi;
                var yetkiGrubu = db.t_yetkiGrubu.ToList();
                var mevcutYetki = db.t_yetkiGrubu.FirstOrDefault(x => x.id == kontakt.yetkiGrubuID);
                ViewBag.YetkiGrubu = yetkiGrubu;
                ViewBag.MevcutYetki = mevcutYetki;
                var roller = db.t_roller.ToList();
                ViewBag.Roller = roller;
                var rolTablosu = db.t_rolTablosu.Where(x => x.contactID == id).Select(x => x.rollerID).ToList();
                ViewBag.RolTablosu = rolTablosu;
                if (kontakt == null)
                {
                    return HttpNotFound();
                }

                return View(kontakt);
            }
        }
        [Authorize(Roles =("Admin"))]
        [HttpPost]
        public ActionResult Guncelle(KontaktGuncelleView model, HttpPostedFileBase Gorsel)
        {
            if (model != null && model.KontaktID != 0)
            {
                var kontakt = db.t_contact.FirstOrDefault(x => x.id == model.KontaktID);
                if (Gorsel != null)
                {
                    if (kontakt.Gorsel == "~/imgs/contact_default/dflt.jpg")
                    {
                        string YeniDosyaAdi = Guid.NewGuid().ToString() + "_" + Gorsel.FileName;
                        string DosyaYolu = Path.Combine(Server.MapPath("~/imgs/contact"), YeniDosyaAdi);
                        Gorsel.SaveAs(DosyaYolu);
                        kontakt.Gorsel = "~/imgs/contact/" + YeniDosyaAdi;
                    }
                    else
                    {
                        string EskiDosyaYolu = Server.MapPath(kontakt.Gorsel);
                        if (System.IO.File.Exists(EskiDosyaYolu))
                        {
   
                            System.IO.File.Delete(EskiDosyaYolu);
                        }

                        string YeniDosyaAdi = Guid.NewGuid().ToString() + "_" + Gorsel.FileName;
                        string DosyaYolu = Path.Combine(Server.MapPath("~/imgs/contact"), YeniDosyaAdi);
                        Gorsel.SaveAs(DosyaYolu);
                        kontakt.Gorsel = "~/imgs/contact/" + YeniDosyaAdi;
                    }
                }
                if (kontakt.organizasyonMu)
                {
                    if (model.OrganizasyonAdi != null)
                    {
                        kontakt.organizasyonAdi = model.OrganizasyonAdi;
                    }
                    if (model.VergiNo != null)
                    {
                        kontakt.vergiNo = model.VergiNo;
                    }
                    if (model.KayitNo != null)
                    {
                        kontakt.kayitNo = model.KayitNo;
                    }
                    if (model.WebSitesi != null)
                    {
                        kontakt.website = model.WebSitesi;
                    }
                    if (model.Description != null)
                    {
                        kontakt.description = model.Description;
                    }
                }
                else
                {
                    if (model.Isim != null)
                    {
                        kontakt.isim = model.Isim;
                    }
                    if (model.SoyIsim != null)
                    {
                        kontakt.soyAdi = model.SoyIsim;
                    }
                    if (model.OrganizasyonAtama.ToString() != null)
                    {
                        if (model.OrganizasyonAtama == 0)
                        {
                            kontakt.organizasyonAtama = 0;
                        }
                        else
                        {
                            kontakt.organizasyonAtama = model.OrganizasyonAtama;
                        }
                        
                    }
                    
                }
                if (model.CepTelefonu != null)
                {
                    kontakt.cepTelefonu = model.CepTelefonu;
                }
                if (model.Email != null)
                {
                    kontakt.e_mail = model.Email;
                }
                if (model.Email2 != null)
                {
                    kontakt.e_mail2 = model.Email2;
                }
                if (model.IsTelefonu != null)
                {
                    kontakt.isTelefonu = model.IsTelefonu;
                }
                if (model.IsTelefonu2 != null)
                {
                    kontakt.isTelefonu2 = model.IsTelefonu2;
                }
                if (model.TercihEdilenIletisimID != 0)
                {
                    kontakt.tercihEdilenIletisim = model.TercihEdilenIletisimID;
                }
                if (model.RollerUnvanlar != null)
                {
                    var roller = db.t_rolTablosu.Where(x => x.contactID == model.KontaktID).ToList();
                    
                    foreach (var rol in model.RollerUnvanlar)
                    {
                        bool rolAyikla = roller.Any(x => x.rollerID == rol);
                        if (!rolAyikla)
                        {
                            var yeniRol = new t_rolTablosu()
                            {
                                contactID = model.KontaktID,
                                rollerID = rol
                            };
                            db.t_rolTablosu.Add(yeniRol);
                        }
                    }
                    var kaldirilacaklar = roller.Where(x => !model.RollerUnvanlar.Contains(x.rollerID) && x.contactID == model.KontaktID).ToList();
                    db.t_rolTablosu.RemoveRange(kaldirilacaklar);
                }
                else
                {
                    var topluSil = db.t_rolTablosu.Where(x => x.contactID == model.KontaktID).ToList();
                    db.t_rolTablosu.RemoveRange(topluSil);
                }
                db.SaveChanges();
                return RedirectToAction("Guncelle", new {id = model.KontaktID });
            }
            else
            {
                return HttpNotFound();
            }

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
                var mevcutIletisimListesi = db.t_tercihEdilenIletisim.FirstOrDefault(x => x.id == kontakt.tercihEdilenIletisim);
                ViewBag.MevcutIletisimListesi = mevcutIletisimListesi;
                var mevcutYetki = db.t_yetkiGrubu.FirstOrDefault(x => x.id == kontakt.yetkiGrubuID);
                ViewBag.MevcutYetki = mevcutYetki;


                if (kontakt == null)
                {
                    return HttpNotFound();
                }

                return View(kontakt);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Sil(int contactId)
        {
            var kontakt = db.t_contact.Find(contactId); 
            if (kontakt == null ||kontakt.id != 1 || kontakt.yetkiGrubuID != null)                   
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