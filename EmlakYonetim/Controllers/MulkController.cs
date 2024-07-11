using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmlakYonetim.Models.ViewModels;
using System.Data.Entity;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace EmlakYonetim.Controllers
{
    public class MulkController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Mulkler
        [AllowAnonymous]
        public ActionResult Index()
        {

            var mulkListesi = db.t_mulk.ToList();
            var MulkViewList = new List<MulkThumbnailModel>();

            foreach (var mulk in mulkListesi)
            {
               
                
                {
                    var ilkFoto = db.t_mulkIMGs.FirstOrDefault(f => f.mulkID == mulk.id);
                    var satisTipi = db.t_mulk.Where(s => s.id == mulk.id).Select(x => x.t_satisTipi.satisTipiAdi).FirstOrDefault();
                    var viewModel = new MulkThumbnailModel
                    {
                        MulkId = mulk.id,
                        Baslik = mulk.mulkBaslik ?? string.Empty,
                        Aciklama = mulk?.aciklama ?? string.Empty,
                        FotoUrl = ilkFoto?.imagePath ?? string.Empty,
                        Fiyat = mulk.fiyat,
                        SatisTipi = satisTipi,
                        SaticiID = mulk.saticiID

                    };
                    MulkViewList.Add(viewModel);
                }

            }



            return View(MulkViewList);
        }

        public ActionResult Ekle()
        {
            ViewBag.Mulktipleri = new SelectList(db.t_mulkTipi, "id", "tipAdi");
            ViewBag.SatisTipleri = new SelectList(db.t_satisTipi, "id", "satisTipiAdi");
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(int mulkTipiID, int satisTipiID)
        {
            TempData["MulkTipiID"] = mulkTipiID;
            TempData["SatisTipiID"] = satisTipiID;

            return RedirectToAction("Secim", "Mulk");
        }

        public ActionResult Secim()
        {
            int? mulkTipiID = TempData["MulkTipiID"] as int?;
            int? satisTipiID = TempData["SatisTipiID"] as int?;
            MulkViewModel model = new MulkViewModel();
            model.Ozellikler = db.t_ozellik.ToList();
            model.BaslikOzellikler = db.t_mulkTipiOzellik.Where(m => m.mulkTipiID == mulkTipiID).Select(m => m.t_ozellik).ToList();
            var satisTipiModuller = db.t_satisTipiModul.Where(s => s.satisTipiID == satisTipiID).Select(m => m.modulID).ToList();
            model.Moduller = db.t_mulkTipiModul.Where(m => m.mulktipiID == mulkTipiID && satisTipiModuller.Contains(m.modulID)).Select(m => m.t_modul).ToList();
            model.Gorseller = new List<HttpPostedFileBase>();
            ViewBag.MulkTipiAdi = db.t_mulkTipi.FirstOrDefault(m => m.id == mulkTipiID)?.tipAdi;
            ViewBag.SatisTipiAdi = db.t_satisTipi.FirstOrDefault(s => s.id == satisTipiID)?.satisTipiAdi;
            return View(model);
        }
        [HttpPost]
        public ActionResult MulkEkle(MulkViewModel model)
        {
            var yeniAdres = new t_mulkAdres()
            {
                sokakCadde = model.SokakCadde,
                apartmanNo = model.ApartmanNo,
                il = model.Il,
                ilce = model.Ilce,
                ulke = model.Ulke,
                postaKodu = model.PostaKodu,
                lat = model.Latitude,
                lonq = model.Longitude
            };
            db.t_mulkAdres.Add(yeniAdres);
            db.SaveChanges();

            var yeniMulk = new t_mulk()
            {
                
                satisTipiID = model.satisTipiID,
                mulkTipiID = model.mulkTipiID,
                mulkBaslik = model.MulkBaslik,
                aciklama = model.MulkAciklama,
                fiyat = model.MulkFiyat,
                adresID = yeniAdres.id,
                saticiID = model.SaticiID
               
            };
            db.t_mulk.Add(yeniMulk);
            db.SaveChanges();
            if (model.Gorseller != null && model.Gorseller.Count > 0)
            {
                foreach (var photo in model.Gorseller)
                {
                    if (photo != null && photo.ContentLength > 0)
                    {
                       
                        using (var uploadedImage = Image.FromStream(photo.InputStream))
                        {
                           
                            using (var resizedImage = new Bitmap(350, 600))
                            using (var graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(uploadedImage, 0, 0, 350, 600);

                               
                                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                                string path = Path.Combine(Server.MapPath("~/imgs/mulk"), fileName);
                                resizedImage.Save(path, ImageFormat.Png); 

                                
                                var yeniImg = new t_mulkIMGs()
                                {
                                    mulkID = yeniMulk.id, 
                                    imagePath = "~/imgs/mulk/" + fileName
                                };

                                db.t_mulkIMGs.Add(yeniImg);
                            }
                        }
                    }
                }
            }
            var yeniMulkID = yeniMulk.id;
            if (model.GirilenModuller != null)
            {
                var i = 0;
                foreach (var input in model.GirilenModuller)
                {
                    var yeniMulkModul = new t_mulkModul()
                    {
                        mulkID = yeniMulk.id,
                        deger = input,
                        modulID = model.InputModulID[i]

                    };
                    i++;
                    db.t_mulkModul.Add(yeniMulkModul);
                    
                }
            }
            if (model.SecilenModuller != null)
            {
                var i = 0;
                foreach (var selected in model.SecilenModuller)
                {
                    var yeniMulkModul = new t_mulkModul()
                    {
                        mulkID = yeniMulk.id,
                        deger = selected,
                        modulID = model.DropDownModulID[i]
                    };
                    i++;
                    db.t_mulkModul.Add(yeniMulkModul);
                    
                }
            }
            if (model.SecilenOzellikler != null)
            {
                foreach (var ozellik in model.SecilenOzellikler)
                {
                    var yeniMulkOzellik = new t_mulkOzellik()
                    {
                        mulkID = yeniMulk.id,
                        ozellikID = ozellik

                    };
                    db.t_mulkOzellik.Add(yeniMulkOzellik);
                } 
            }
            db.SaveChanges();
            return RedirectToAction("IlanDetaylar", new { mulkID = yeniMulkID});
        }
        public ActionResult IlanSil(int MulkID)
        {
            var mulk = db.t_mulk.FirstOrDefault(x => x.id == MulkID);
            var mulkOzellik = db.t_mulkOzellik.Where(x => x.mulkID == MulkID).ToList();
            var mulkModul = db.t_mulkModul.Where(x => x.mulkID == MulkID).ToList();
            var mulkIMGs = db.t_mulkIMGs.Where(x => x.mulkID == MulkID).ToList();
            var adres = db.t_mulkAdres.FirstOrDefault(x => x.id == mulk.adresID);
            if (mulkIMGs != null)
            {
                foreach (var foto in mulkIMGs)
                {
                    string Kaldir = Server.MapPath(foto.imagePath);
                    if (System.IO.File.Exists(Kaldir))
                    {

                        System.IO.File.Delete(Kaldir);
                    }

                }
                db.t_mulkIMGs.RemoveRange(mulkIMGs);
                
            }
           
            db.t_mulkOzellik.RemoveRange(mulkOzellik);
            db.t_mulkModul.RemoveRange(mulkModul);
            db.t_mulk.Remove(mulk);
            db.SaveChanges();
            db.t_mulkAdres.Remove(adres);

            db.SaveChanges();
            
            return View("Index");
        }
    
        // İlanların düzenleneceği actionlar...
        public ActionResult IlanDuzenle(int MulkID)
        {
            var mulk = db.t_mulk.FirstOrDefault(m => m.id == MulkID);
            var satisTipi = db.t_satisTipiModul.Where(x => x.satisTipiID == mulk.satisTipiID).Select(x => x.t_modul).ToList();
            var fotograflar = db.t_mulkIMGs.Where(f => f.mulkID == MulkID).ToList();
            var mulkOzellikler = db.t_mulkOzellik.Where(x => x.mulkID == MulkID).Select(o => o.t_ozellik).ToList();
            var mulkModuller = db.t_mulkModul.Where(y => y.mulkID == MulkID).ToList();
            var tumOzellikler = db.t_ozellik.ToList();
            var mulkTipiOzellik = db.t_mulkTipiOzellik.Include("t_ozellik").ToList();
            var mulkTipiModul = db.t_mulkTipiModul.Where(x => x.mulktipiID == mulk.mulkTipiID).ToList();
            var adres = db.t_mulk.Where(x => x.id == MulkID).Select(a => a.t_mulkAdres).FirstOrDefault();
            var kullanicilar = db.t_kullanici.Include("t_contact").ToList();
            var mevcutKullanici = db.t_mulk
            .Where(m => m.id == MulkID)
            .Select(m => m.t_contact)
            .FirstOrDefault();


            var model = new MulkDuzenleViewModel()
            {
                MulkID = MulkID,
                Fotograflar = fotograflar,
                Mulk = mulk, 
                MulkOzellikler = mulkOzellikler,
                MulkModuller = mulkModuller,
                TumOzellikler = tumOzellikler,
                TumModuller = satisTipi,
                MulkTipiOzellik = mulkTipiOzellik,
                MulkTipiID = mulk.mulkTipiID,
                SokakCadde = adres.sokakCadde,
                ApartmanNo = adres.apartmanNo,
                Il = adres.il,
                Ilce = adres.ilce,
                Ulke = adres.ulke,
                PostaKodu = adres.postaKodu,
                Latitude = adres.lat,
                Longitude = adres.lonq,
                Kullanicilar = kullanicilar,
                MevcutKullanici = mevcutKullanici
            };
           
            return View(model);
        }
        public ActionResult SaticiAta(int? SaticiID, int? mulkID)
        {
            if (SaticiID != null && mulkID != null)
            {
                var mulkSatici = db.t_mulk.FirstOrDefault(x => x.id == mulkID);
                mulkSatici.saticiID = SaticiID.Value;
                db.SaveChanges();
                return RedirectToAction("IlanDuzenle", new { mulkID = mulkID.Value });
            }
            else
            {
                return RedirectToAction("IlanDuzenle", new { mulkID = mulkID.Value });
            }

            
        }
        public ActionResult TekFotoSil(int FotoID)
        {
            var foto = db.t_mulkIMGs.FirstOrDefault(x => x.id == FotoID);
            var mulkID = foto.mulkID;
            if (foto != null)
            {
                var filePath = Server.MapPath(foto.imagePath);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                db.t_mulkIMGs.Remove(foto);
                db.SaveChanges();
            }

            return RedirectToAction("IlanDuzenle", new { mulkID = mulkID });
        }
        public ActionResult IlanFotolariSil(int MulkID)
        {
            var ilanFotolari = db.t_mulkIMGs.Where(x => x.mulkID == MulkID).ToList();
            var mulkID = MulkID;
            if (ilanFotolari.Any())
            {
                foreach (var foto in ilanFotolari)
                {
                    var filePath = foto.imagePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                db.t_mulkIMGs.RemoveRange(ilanFotolari);
                db.SaveChanges();

                return RedirectToAction("IlanDuzenle", new { mulkID = mulkID });
            }
            else
            {
                return HttpNotFound();
            }

        }
        public ActionResult IlanFotoEkle( List<HttpPostedFileBase> Fotograflar, int MulkID)
        {
            var yeniFotolar = db.t_mulkIMGs.ToList();
            var mulkID = MulkID;
            if (Fotograflar != null && Fotograflar.Count > 0)
            {
                foreach (var foto in Fotograflar)
                {
                    if (foto != null && foto.ContentLength > 0)
                    {

                        using (var uploadedImage = Image.FromStream(foto.InputStream))
                        {

                            using (var resizedImage = new Bitmap(350, 600))
                            using (var graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(uploadedImage, 0, 0, 350, 600);


                                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                                string path = Path.Combine(Server.MapPath("~/imgs/mulk"), fileName);
                                resizedImage.Save(path, ImageFormat.Png);


                                var yeniImg = new t_mulkIMGs()
                                {
                                    mulkID = mulkID,
                                    imagePath = "~/imgs/mulk/" + fileName
                                };

                                db.t_mulkIMGs.Add(yeniImg);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                return RedirectToAction("IlanDuzenle", new { mulkID = mulkID });
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        public ActionResult AnaFotografiDegistir(int MulkID, int FotoID)
        {
            var anaFotograf = db.t_mulkIMGs.Where(x => x.mulkID == MulkID).FirstOrDefault();
            var yeniAnaFotograf = db.t_mulkIMGs.Where(f => f.id == FotoID).FirstOrDefault();

            if (MulkID != 0)
            {
                var tempFoto = new t_mulkIMGs()
                {
                    imagePath = anaFotograf.imagePath
                };
                anaFotograf.imagePath = yeniAnaFotograf.imagePath;
                yeniAnaFotograf.imagePath = tempFoto.imagePath;
                db.SaveChanges();
                return RedirectToAction("IlanDuzenle",  new { MulkID = MulkID }); 
            }
            else
            {
                return HttpNotFound();
            }
            
        }
        public ActionResult IlanYeniKaydet(MulkDuzenleViewModel model)
        {
            if (model.MulkID != 0)
            {
                var mulkID = model.MulkID;
                var mulkBaslik = model.MulkBaslik;
                var mulkAciklama = model.MulkAciklama;
                var ozellikler = model.SecilenOzellikler;
                var dropDownModuller = model.DropDownModulID;
                var inputModuller = model.InputModulID;
                var girilenDeger = model.GirilenDeger;
                var secilenDeger = model.SecilenDeger;
                var yeniInputModuller = model.YeniInputModulID;
                var yeniDropDownModuller = model.YeniDropDownModulID;
                var yeniGirilenDeger = model.YeniGirilenDeger;
                var yeniSecilenDeger = model.YeniSecilenDeger;
                var sokakCadde = model.SokakCadde;
                var apartmanNo = model.ApartmanNo;
                var ilce = model.Ilce;
                var il = model.Il;
                var ulke = model.Ulke;
                var postaKodu = model.PostaKodu;
                var lonq = model.Longitude;
                var lat = model.Latitude;
                var mevcutOzellikler = db.t_mulkOzellik.ToList();


                if (mulkBaslik != null && mulkAciklama != null)
                {
                    var mevcutMulk = db.t_mulk.FirstOrDefault(x => x.id == mulkID);
                    mevcutMulk.aciklama = mulkAciklama;
                    mevcutMulk.mulkBaslik = mulkBaslik;
                }
                if (mulkID != 0)
                {
                    var mulkAdres = db.t_mulk.Where(x => x.id == mulkID).Select(x => x.t_mulkAdres).FirstOrDefault();
                    if (sokakCadde != null)
                    {
                        mulkAdres.sokakCadde = sokakCadde;
                    }
                    if (apartmanNo != null)
                    {
                        mulkAdres.apartmanNo = apartmanNo;
                    }
                    if (il != null)
                    {
                        mulkAdres.il = il;
                    }
                    if (ilce != null)
                    {
                        mulkAdres.ilce = ilce;
                    }
                    if (ulke != null)
                    {
                        mulkAdres.ulke = ulke;
                    }
                    if (postaKodu != null)
                    {
                        mulkAdres.postaKodu = postaKodu;
                    }
                    if (lat != null)
                    {
                        mulkAdres.lat = lat;
                    }
                    if (lonq != null)
                    {
                        mulkAdres.lonq = lonq;
                    }
                }

                if (ozellikler != null)
                {
                    foreach (var ozellikid in ozellikler)
                    {
                        bool ozellikAyikla = mevcutOzellikler.Any(x => x.ozellikID == ozellikid && x.mulkID == mulkID);
                        if (!ozellikAyikla)
                        {
                            var yeniMulkOzellik = new t_mulkOzellik()
                            {
                                mulkID = mulkID,
                                ozellikID = ozellikid
                            };
                            db.t_mulkOzellik.Add(yeniMulkOzellik);
                        }

                    }
                    var kaldirilacaklar = mevcutOzellikler.Where(x => !ozellikler.Contains(x.ozellikID) && x.mulkID == mulkID).ToList();
                    db.t_mulkOzellik.RemoveRange(kaldirilacaklar);
                }
                else
                {
                    var topluSil = db.t_mulkOzellik.Where(x => x.mulkID == mulkID).ToList();
                    db.t_mulkOzellik.RemoveRange(topluSil);
                }

                if (dropDownModuller != null)
                {
                    for (int i = 0; i < dropDownModuller.Count; i++)
                    {
                        var modulid = dropDownModuller[i];
                        var modul = db.t_mulkModul.FirstOrDefault(x => x.modulID == modulid && x.mulkID == mulkID);
 
                        
                        if (modul != null && !string.IsNullOrEmpty(secilenDeger[i]))
                        {
                            modul.deger = secilenDeger[i];
                            
                        }
                       
                       
                    }
                }


                if (inputModuller != null)
                {
                    for (int i = 0; i < inputModuller.Count; i++)
                    {
                        var modulid = inputModuller[i];
                        var modul = db.t_mulkModul.FirstOrDefault(x => x.modulID == modulid && x.mulkID == mulkID);

                        if (modul != null && !string.IsNullOrEmpty(girilenDeger[i]))
                        {
                            modul.deger = girilenDeger[i];
                           
                        }
                    }
                }
                if (yeniDropDownModuller != null)
                {
                    for (int i = 0; i < yeniDropDownModuller.Count; i++)
                    {
                        var modulid = yeniDropDownModuller[i];
                        var modul = db.t_mulkModul.FirstOrDefault(x => x.modulID == modulid && x.mulkID == mulkID);
                        if (modul == null && !string.IsNullOrEmpty(yeniSecilenDeger[i]))
                        {
                            var yeniModul = new t_mulkModul()
                            {
                                mulkID = mulkID,
                                modulID = modulid,
                                deger = yeniSecilenDeger[i]
                            };
                            db.t_mulkModul.Add(yeniModul);
                        }
                    }
                }
                if (yeniInputModuller != null)
                {
                    for (int i = 0; i < yeniInputModuller.Count; i++)
                    {
                        var modulid = yeniInputModuller[i];
                        var modul = db.t_mulkModul.FirstOrDefault(x => x.modulID == modulid && x.mulkID == mulkID);
                        if (modul == null && !string.IsNullOrEmpty(yeniGirilenDeger[i]))
                        {
                            var yeniModul = new t_mulkModul()
                            {
                                mulkID = mulkID,
                                modulID = modulid,
                                deger = yeniGirilenDeger[i]
                            };
                            db.t_mulkModul.Add(yeniModul);
                        }
                    }
                }

                db.SaveChanges();
            }
            
            else
            {
                HttpNotFound();
            }

            return RedirectToAction("IlanDuzenle", new { mulkID = model.MulkID });
        }
        // Askıya alınan özellikler
        public ActionResult FotograflariDuzenle(int mulkID)
        {
            var Gorseller = db.t_mulkIMGs.Where(f => f.mulkID == mulkID).ToList();
            return View(Gorseller);
        }

        [HttpPost]
        public ActionResult KirpVeKaydet(int id, string imagePath, string croppedImage)
        {
           
            byte[] imageBytes = Convert.FromBase64String(croppedImage.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", ""));

           
            string filePath = Server.MapPath(imagePath);
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            
            return Json(new { success = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult IlanDetaylar(int mulkID)
        {
            var moduller = db.t_mulkModul
                .Where(m => m.mulkID == mulkID)
                .Join(db.t_modul,
                      mulkModul => mulkModul.modulID,
                      modul => modul.id,
                      (mulkModul, modul) => new ModulViewModel
                      {
                          ModulAdi = modul.modulAdi,
                          Deger = mulkModul.deger
                      })
                .ToList();
            var detaylar = db.t_mulk.Where(m => m.id == mulkID).FirstOrDefault();
            var gorseller = db.t_mulkIMGs.Where(m => m.mulkID == mulkID).ToList();
            var ozellikler = db.t_mulkOzellik.Where(m => m.mulkID == mulkID).Select(o => o.t_ozellik).ToList();
            var anaOzellikler = db.t_mulk.Where(m => m.id == mulkID)
                .Join(db.t_mulkTipiOzellik, m => m.mulkTipiID, mto => mto.mulkTipiID, (m, mto) => mto.t_ozellik).ToList();
            var adres = db.t_mulk.Where(x => x.id == mulkID).Select(a => a.t_mulkAdres).FirstOrDefault();
            var mulk = db.t_mulk.FirstOrDefault(x => x.id == mulkID);
            var saticiBilgileri = db.t_contact.FirstOrDefault(x => x.id == mulk.saticiID);
            ViewBag.Ozellikler = ozellikler;
            ViewBag.AnaOzellikler = anaOzellikler;
            ViewBag.Detaylar = detaylar;
            ViewBag.Moduller = moduller;
            ViewBag.SatisTipi = db.t_mulk.Where(x => x.id == mulkID).Select(x => x.t_satisTipi.satisTipiAdi).FirstOrDefault();
            ViewBag.Adres = adres;
            ViewBag.SaticiBilgileri = saticiBilgileri;
     
            return View(gorseller);
        }
    }
}
