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
    
    public partial class t_satisTipi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_satisTipi()
        {
            this.t_mulk = new HashSet<t_mulk>();
            this.t_satisTipiModul = new HashSet<t_satisTipiModul>();
        }
    
        public int id { get; set; }
        public string satisTipiAdi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_mulk> t_mulk { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_satisTipiModul> t_satisTipiModul { get; set; }
    }
}
