//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.comment = new HashSet<comment>();
            this.chat = new HashSet<chat>();
            this.like = new HashSet<like>();
            this.tai_lieu = new HashSet<tai_lieu>();
            this.user_group = new HashSet<user_group>();
        }
    
        public string uid { get; set; }
        public string password { get; set; }
        public string ho_ten { get; set; }
        public string email { get; set; }
        [Required(ErrorMessage = "Trường địa chỉ không được để trống!")]
        public string dia_chi { get; set; }
        public string ma_lop { get; set; }
        public string role { get; set; }
        public string anh { get; set; }
        public Nullable<bool> gioi_tinh { get; set; }
        public string gioi_thieu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chat> chat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<like> like { get; set; }
        public virtual lop lop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tai_lieu> tai_lieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_group> user_group { get; set; }
    }
}
