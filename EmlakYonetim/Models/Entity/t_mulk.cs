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
    
    public partial class t_mulk
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_mulk()
        {
            this.t_mulkModul = new HashSet<t_mulkModul>();
            this.t_mulkOzellik = new HashSet<t_mulkOzellik>();
            this.t_mulkIMGs = new HashSet<t_mulkIMGs>();
        }
    
        public int id { get; set; }
        public string mulkBaslik { get; set; }
        public string aciklama { get; set; }
        public int mulkTipiID { get; set; }
        public int satisTipiID { get; set; }
        public string fiyat { get; set; }
        public Nullable<int> adresID { get; set; }
    
        public virtual t_mulkTipi t_mulkTipi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_mulkModul> t_mulkModul { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_mulkOzellik> t_mulkOzellik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_mulkIMGs> t_mulkIMGs { get; set; }
        public virtual t_satisTipi t_satisTipi { get; set; }
        public virtual t_mulkAdres t_mulkAdres { get; set; }
    }
}