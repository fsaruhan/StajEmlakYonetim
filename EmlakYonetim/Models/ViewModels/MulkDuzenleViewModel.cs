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
        public int MulkTipiID { get; set; }
    } 
}