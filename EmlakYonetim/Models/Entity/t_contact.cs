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
    
    public partial class t_contact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_contact()
        {
            this.t_contactDosyalari = new HashSet<t_contactDosyalari>();
            this.t_iletisimIzinKayit = new HashSet<t_iletisimIzinKayit>();
            this.t_rolTablosu = new HashSet<t_rolTablosu>();
        }
    
        public int id { get; set; }
        public Nullable<int> Ulke { get; set; }
        public string isim { get; set; }
        public string soyAdi { get; set; }
        public string organizasyonAdi { get; set; }
        public Nullable<int> vergiNo { get; set; }
        public Nullable<int> kayitNo { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public int organizasyonAtama { get; set; }
        public string e_mail { get; set; }
        public string e_mail2 { get; set; }
        public Nullable<int> isTelefonu { get; set; }
        public Nullable<int> isTelefonu2 { get; set; }
        public Nullable<int> cepTelefonu { get; set; }
        public Nullable<int> adresID { get; set; }
        public Nullable<int> tercihEdilenIletisim { get; set; }
        public Nullable<int> yetkiGrubuID { get; set; }
        public bool organizasyonMu { get; set; }
        public string Gorsel { get; set; }
    
        public virtual t_adres t_adres { get; set; }
        public virtual t_tercihEdilenIletisim t_tercihEdilenIletisim { get; set; }
        public virtual t_uyrukUlke t_uyrukUlke { get; set; }
        public virtual t_yetkiGrubu t_yetkiGrubu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_contactDosyalari> t_contactDosyalari { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_iletisimIzinKayit> t_iletisimIzinKayit { get; set; }
        public virtual t_kullanici t_kullanici { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_rolTablosu> t_rolTablosu { get; set; }
    }
}