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
    
    public partial class t_mulkAdres
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_mulkAdres()
        {
            this.t_mulk = new HashSet<t_mulk>();
        }
    
        public int id { get; set; }
        public string sokakCadde { get; set; }
        public string apartmanNo { get; set; }
        public string il { get; set; }
        public string ilce { get; set; }
        public string ulke { get; set; }
        public string postaKodu { get; set; }
        public string lat { get; set; }
        public string lonq { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_mulk> t_mulk { get; set; }
    }
}
