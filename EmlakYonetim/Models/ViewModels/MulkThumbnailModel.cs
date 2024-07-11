using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class MulkThumbnailModel
    {
        public int MulkId { get; set; }
        public string Baslik { get; set; }
        public string  Aciklama { get; set; }
        public string FotoUrl { get; set; }
        public string SatisTipi { get; set; }
        public string Fiyat { get; set; }
        public int SaticiID { get; set; }
    } 
}