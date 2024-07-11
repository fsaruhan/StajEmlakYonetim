using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class MulkViewModel

    {
   
        public int mulkTipiID { get; set; }
        public int satisTipiID { get; set; }
        public string MulkBaslik { get; set; }
        public string MulkAciklama { get; set; }
        public string MulkFiyat { get; set; }
        public List<t_ozellik> BaslikOzellikler { get; set; }
        public  List<t_satisTipi> SatisTipleri { get; set; }
        public List<t_ozellik> Ozellikler { get; set; }
        public List <t_modul> Moduller { get; set; }
        public List <int> SecilenOzellikler { get; set; }
        public List <string> SecilenModuller { get; set; }
        public List <string> GirilenModuller { get; set; }
        public List <int> InputModulID { get; set; }
        public List<int> DropDownModulID { get; set; }
        public int SaticiID { get; set; }
        public List<HttpPostedFileBase> Gorseller { get; set; }
        public string SokakCadde { get; set; }
        public string ApartmanNo { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Ulke { get; set; }
        public string PostaKodu { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }   
    }   
}
