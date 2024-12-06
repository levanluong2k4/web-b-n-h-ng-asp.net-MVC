

namespace banhang_64131236.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietGioHang
    {
        public int MaGioHang { get; set; }
        public string MaHH { get; set; }
        public int SoLuong { get; set; }
    
        public virtual GioHang GioHang { get; set; }
        public virtual HangHoa HangHoa { get; set; }
    }
}
