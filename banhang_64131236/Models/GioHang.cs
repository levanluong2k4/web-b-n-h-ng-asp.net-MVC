

namespace banhang_64131236.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GioHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GioHang()
        {
            this.ChiTietGioHangs = new HashSet<ChiTietGioHang>();
        }
    
        public int MaGioHang { get; set; }
        public string MaKH { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
