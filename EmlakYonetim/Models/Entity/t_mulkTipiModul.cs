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
    
    public partial class t_mulkTipiModul
    {
        public int id { get; set; }
        public int mulkTipiID { get; set; }
        public int modulID { get; set; }
    
        public virtual t_mulkTipi t_mulkTipi { get; set; }
        public virtual t_modul t_modul { get; set; }
    }
}
