//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyGiangDay.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class LopHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopHoc()
        {
            this.LopHocMonHocs = new HashSet<LopHocMonHoc>();
        }
    
        public string MaLop { get; set; }
        public string MaCTDT { get; set; }
        public string TenLop { get; set; }
        public string MoTa { get; set; }
    
        public virtual CTDT CTDT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopHocMonHoc> LopHocMonHocs { get; set; }
    }
}
