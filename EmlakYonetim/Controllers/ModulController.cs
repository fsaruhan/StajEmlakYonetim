using EmlakYonetim.Models.Entity;
using EmlakYonetim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmlakYonetim.Controllers
{
    public class ModulController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Modul
        public ActionResult Index()
        {
            var moduller = db.t_modul.ToList();
            var mulkTipiModul = db.t_mulkTipiModul.Include("t_mulkTipi").Include("t_modul").ToList();
            var mulkTipi = db.t_mulkTipi.ToList();
            var satisTipleri = db.t_satisTipi.ToList();
            var dahiliSatis = db.t_satisTipiModul.ToList();
            
            var model = new ModulViewModel()
            {
                MulkTipiModuller = mulkTipiModul,
                Moduller = moduller,
                MulkTipleri = mulkTipi,
                SatisTipleri = satisTipleri,
                DahiliSatisTipleri = dahiliSatis
             
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ModulEkle(ModulViewModel model)
        {
            var modulAdi = model.ModulAdi;
            var degerler = model.Deger;
            var mulkTipi = model.MulkTipiID;
            var satisTipleri = model.SatisTipiIDler;

            if (modulAdi != null && mulkTipi != 0)
            {
                var yeniModul = new t_modul()
                {
                    modulAdi = modulAdi,
                    degerler = degerler,

                };
                db.t_modul.Add(yeniModul);
                var yeniModulMulkTipi = new t_mulkTipiModul()
                {
                    modulID = yeniModul.id,
                    mulktipiID = mulkTipi
                }; 
                db.t_mulkTipiModul.Add(yeniModulMulkTipi);

                if (satisTipleri != null)
                {
                    foreach (var satisTipi in satisTipleri)
                    {
                        var yeniModulSatisTipi = new t_satisTipiModul()
                        {
                            modulID = yeniModul.id,
                            satisTipiID = satisTipi

                        };
                        db.t_satisTipiModul.Add(yeniModulSatisTipi);
                    }
                }
                else
                {
                    db.SaveChanges();
                }

                
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            else
            {
               return HttpNotFound();
            }

        }
        public ActionResult ModulSil(int? ModulID)
        {
            var modul = db.t_modul.FirstOrDefault(i => i.id == ModulID);
            var mulkTipiModul = db.t_mulkTipiModul.FirstOrDefault(m => m.modulID == ModulID);
            var mulkModul = db.t_mulkModul.Where(m => m.modulID == ModulID);
            var satisTipiModul = db.t_satisTipiModul.Where(m => m.modulID == ModulID);

        
            if (ModulID != null)
            {
                
                db.t_mulkTipiModul.Remove(mulkTipiModul);
                db.t_modul.Remove(modul);
                db.t_satisTipiModul.RemoveRange(satisTipiModul);
                if (mulkModul != null)
                {
                    db.t_mulkModul.RemoveRange(mulkModul);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            else
            {
                return HttpNotFound();
            }

            
        }
        public ActionResult ModulDuzenle(int? ModulID)
        {
            var modul = db.t_modul.FirstOrDefault(m => m.id == ModulID);
            var mulkTipi = db.t_mulkTipiModul.Where(m => m.modulID == ModulID).Select(x => x.t_mulkTipi).FirstOrDefault();
            var mulkTipleri = db.t_mulkTipi.ToList();
            var dahiliSatisTipleri = db.t_satisTipiModul.Where(x => x.modulID == ModulID).ToList();
            var satisTipleri = db.t_satisTipi.ToList();

            if (ModulID != null)
            {
                var model = new ModulViewModel()
                {
                    Deger = modul.degerler,
                    ModulAdi = modul.modulAdi,
                    ModulID = modul.id,
                    MulkTipiAdi = mulkTipi.tipAdi,
                    MulkTipleri = mulkTipleri,
                    DahiliSatisTipleri = dahiliSatisTipleri,
                    SatisTipleri = satisTipleri
                };
                return View(model);
            }

            else
            {
                return HttpNotFound();
            }
           
        }
        public ActionResult ModulKaydet(ModulViewModel model)
        {
            var modulID = model.ModulID;
            var mulkTipiID = model.MulkTipiID;
            var modulAdi = model.ModulAdi;
            var degerDrop = model.DegerDrop;
            var degerInput = model.DegerInput;
            var satisTipleri = model.SatisTipiIDler;
            var mevcutSatisTipleri = db.t_satisTipiModul.Where(x => x.modulID == modulID);

            if (satisTipleri != null)
            {
                foreach (var satisTipi in satisTipleri)
                {
                   
                    bool mevcutSatisTipiVar = mevcutSatisTipleri.Any(x => x.satisTipiID == satisTipi);

                    if (!mevcutSatisTipiVar)
                    {
                        
                        var yeniSatisTipiModul = new t_satisTipiModul()
                        {
                            modulID = modulID,
                            satisTipiID = satisTipi
                        };

                        db.t_satisTipiModul.Add(yeniSatisTipiModul);
                    }
                }

                var kaldirilacaklar = mevcutSatisTipleri.Where(x => !satisTipleri.Contains(x.satisTipiID)).ToList();
                db.t_satisTipiModul.RemoveRange(kaldirilacaklar);

            
                db.SaveChanges();
            }
            else
            {
                var yenile = db.t_satisTipiModul.Where(s => s.modulID == modulID);
                db.t_satisTipiModul.RemoveRange(yenile);
                db.SaveChanges();
            }

            if (mulkTipiID != 0)
            {
                var yeniMulkTipi = db.t_mulkTipiModul.FirstOrDefault(y => y.modulID == modulID);
                yeniMulkTipi.mulktipiID = mulkTipiID;
                db.SaveChanges();
            }
            if (modulAdi != null)
            {
                var yeniModulAdi = db.t_modul.FirstOrDefault(m => m.id == modulID);
                yeniModulAdi.modulAdi = modulAdi;
                db.SaveChanges();
            }
            if (degerDrop != null)
            {
                var yeniDegerler = db.t_modul.FirstOrDefault(m => m.id == modulID);
                yeniDegerler.degerler = degerDrop;
                db.SaveChanges();
            }
            else
            {
                var yeniDegeler = db.t_modul.FirstOrDefault(m => m.id == modulID);
                yeniDegeler.degerler = null;
                db.SaveChanges();
            }
            if (degerInput != null)
            {
                var yeniDegerler = db.t_modul.FirstOrDefault(m => m.id == modulID);
                yeniDegerler.degerler = degerInput;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
    }
}