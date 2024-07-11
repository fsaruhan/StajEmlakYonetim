using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EmlakYonetim.Models.ViewModels
{
    public class MulkDuzenleViewModel
    {
        public int MulkID { get; set; }
        public List<t_mulkIMGs> Fotograflar { get; set; }
        public t_mulk Mulk { get; set; }
        public List<t_modul> TumModuller { get; set; }
        public List<t_ozellik> MulkOzellikler { get; set; }
        public List<t_mulkModul> MulkModuller { get; set; }
        public List <t_ozellik> TumOzellikler { get; set; }
        public List <t_mulkTipiOzellik> MulkTipiOzellik { get; set; }
        public List <t_satisTipi> SatisTipi { get; set; }
        public List<int> SecilenOzellikler { get; set; }
        public string MulkBaslik { get; set; }
        public string MulkAciklama { get; set; }
        public List<int> DropDownModulID { get; set; }
        public List<int> InputModulID { get; set; }
        public List<string> GirilenDeger { get; set; }
        public List<string> SecilenDeger { get; set; }
        public List<int> YeniDropDownModulID { get; set; }
        public List<int> YeniInputModulID { get; set; }
        public List<string> YeniGirilenDeger { get; set; }
        public List<string> YeniSecilenDeger { get; set; }
        public int MulkTipiID { get; set; }
        public string SokakCadde { get; set; }
        public string ApartmanNo { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Ulke { get; set; }
        public string PostaKodu { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string SaticiFoto { get; set; }
        public List<t_kullanici> Kullanicilar { get; set; }
        public t_contact MevcutKullanici { get; set; }
    } 
}

