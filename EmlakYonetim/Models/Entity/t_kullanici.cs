//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmlakYonetim.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_kullanici
    {
        public int id { get; set; }
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }
        public int yetkiGrubu { get; set; }
    
        public virtual t_contact t_contact { get; set; }
        public virtual t_yetkiGrubu t_yetkiGrubu { get; set; }
    }
}