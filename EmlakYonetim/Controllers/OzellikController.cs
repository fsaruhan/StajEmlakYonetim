using EmlakYonetim.Models.Entity;
using EmlakYonetim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmlakYonetim.Controllers
{
    public class OzellikController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Ozellik
        public ActionResult Index()
        {
            var ozellikler = db.t_ozellik.Where(a => a.ozellikAtama != 0).ToList();
            var anaOzellikler = db.t_ozellik.Where(a => a.ozellikAtama == 0).ToList();
            var mulkTipi = db.t_mulkTipi.ToList();
            var mulkTipiOzellik = db.t_mulkTipiOzellik.Include("t_mulkTipi").ToList();
            var model = new OzellikViewModel
            {
                AnaOzellikler = anaOzellikler,
                Ozellikler = ozellikler,
                MulkTipi = mulkTipi,
                MulkTipiOzellik = mulkTipiOzellik
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Sil(OzellikViewModel model)
        {
            var altOzellikID = model.OzellikID;
            var altOzellikMulk = db.t_mulkOzellik.Where(m => m.ozellikID == altOzellikID).ToList();
            db.t_mulkOzellik.RemoveRange(altOzellikMulk);
            var altOzellik = db.t_ozellik.FirstOrDefault(m => m.id == altOzellikID);
            db.t_ozellik.Remove(altOzellik);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AnaOzellikEkle(string AnaOzellikAdi, int MulkTipiID)
        {
            var anaOzellikAdi = AnaOzellikAdi;
            var mulkTipiID = MulkTipiID;
            if (AnaOzellikAdi != null)
            {
                var yeniAnaOzellik = new t_ozellik()
                {
                    ozellikAtama = 0,
                    ozellikAdi = anaOzellikAdi,
                };
                db.t_ozellik.Add(yeniAnaOzellik);
                var mulkTipiOzellik = new t_mulkTipiOzellik()
                {
                    mulkTipiID = mulkTipiID,
                    ozellikID = yeniAnaOzellik.id
                };
                db.t_mulkTipiOzellik.Add(mulkTipiOzellik);
                db.SaveChanges();
            }
            else
            {
                HttpNotFound();
            }
            return RedirectToAction("Index");
        }
        public ActionResult AnaOzellikKaldir(int AnaOzellikID)
        {

            var anaOzellik = db.t_ozellik.FirstOrDefault(m => m.id == AnaOzellikID);
            var ilgiliOzellikler = db.t_ozellik.Where(o => o.ozellikAtama == AnaOzellikID);
            var ilgiliMulkTipi = db.t_mulkTipiOzellik.FirstOrDefault(m => m.ozellikID == AnaOzellikID);
            var ilgiliMulkOzellikler = db.t_mulkOzellik.Where(m => m.t_ozellik.ozellikAtama == AnaOzellikID);

            db.t_mulkOzellik.RemoveRange(ilgiliMulkOzellikler);
            db.t_mulkTipiOzellik.Remove(ilgiliMulkTipi);
            db.t_ozellik.RemoveRange(ilgiliOzellikler);
            db.t_ozellik.Remove(anaOzellik);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }
        public ActionResult AnaOzellikDuzenle(int AnaOzellikID,string AnaOzellikAdi)
        {
            var anaOzellikGuncelle = db.t_ozellik.FirstOrDefault(x => x.id == AnaOzellikID);

            if (anaOzellikGuncelle != null)
            {
                anaOzellikGuncelle.ozellikAdi = AnaOzellikAdi;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public  ActionResult AltOzellikEkle(OzellikViewModel model)
        {
            var anaOzellikID = model.AnaOzellikID;
            var altOzellikAdi = model.AltOzellikAdi;

            var yeniOzellik = new t_ozellik()
            {
                ozellikAdi = altOzellikAdi,
                ozellikAtama = anaOzellikID
            };
            db.t_ozellik.Add(yeniOzellik);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult OzellikMulkTipiGuncelle(OzellikViewModel model)
        {
            var anaOzellikID = model.AnaOzellikID;
            var mulkTipiID = model.MulkTipiID;

           
            var ozellikMulkTipi = db.t_mulkTipiOzellik.FirstOrDefault(x => x.ozellikID == anaOzellikID);

            if (ozellikMulkTipi != null)
            {
                ozellikMulkTipi.mulkTipiID = mulkTipiID;
                db.SaveChanges();


                TempData["SuccessMessage"] = "Mülk Tipi başarıyla güncellendi.";
            }
            else
            {
               
                TempData["ErrorMessage"] = "Mülk Tipi güncellenirken bir hata meydana geldi.";
            }

            return RedirectToAction("Index");
        }
      

    }
}