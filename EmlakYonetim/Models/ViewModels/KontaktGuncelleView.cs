using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class KontaktGuncelleView
    {
        public int KontaktID { get; set; }
        public string OrganizasyonAdi { get; set; }
        public string VergiNo { get; set; }
        public string KayitNo { get; set; }
        public string WebSitesi { get; set; }
    
        public string CepTelefonu { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string IsTelefonu { get; set; }
        public string IsTelefonu2 { get; set; }
        public string Description { get; set; }
        public string Isim { get; set; }
        public string SoyIsim { get; set; }
        public int OrganizasyonAtama { get; set; }
        public List<int> RollerUnvanlar { get; set; }
        public int TercihEdilenIletisimID { get; set; }
        public int YetkiGrubuID { get; set; }




    }
}