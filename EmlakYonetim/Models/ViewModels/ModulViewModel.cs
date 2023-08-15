using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class ModulViewModel
    {
        public int ModulID { get; set; }
        public int SatisTipiID { get; set; }
        public int MulkTipiID { get; set; }
        public string ModulAdi { get; set; }
        public string Deger { get; set; }
        public string DegerDrop { get; set; }
        public string DegerInput { get; set; }
        public string MulkTipiAdi { get; set; }
        public List<t_satisTipiModul> DahiliSatisTipleri { get; set; }
        public List<t_satisTipi> SatisTipleri{ get; set; }
        public List <int> SatisTipiIDler { get; set; }
        public List<t_mulkTipi> MulkTipleri { get; set; }
        public List<t_modul> Moduller { get; set; }
        public List<t_mulkTipiModul> MulkTipiModuller { get; set; }
    }
}