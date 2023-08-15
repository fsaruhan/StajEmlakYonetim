using EmlakYonetim.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class OzellikViewModel
    {
        public string AltOzellikAdi { get; set; }
        public string AnaOzellikAdi { get; set; }
        public int AnaOzellikID { get; set; }
        public int OzellikID { get; set; }
        public int MulkTipiID { get; set; }
        public List<t_ozellik> AnaOzellikler { get; set; }
        public List<t_ozellik> Ozellikler { get; set; }
        public List<t_mulkTipi> MulkTipi { get; set; }
        public List<t_mulkTipiOzellik> MulkTipiOzellik { get; set; }
    }
}