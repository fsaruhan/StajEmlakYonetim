using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakYonetim.Models.ViewModels
{
    public class KullaniciEkleView
    {
        public int? SecilenKayit { get; set; }
        public int? SecilenYetki { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
    }
}