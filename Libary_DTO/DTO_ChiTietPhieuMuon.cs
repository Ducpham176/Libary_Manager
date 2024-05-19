using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    class DTO_ChiTietPhieuMuon
    {
        public int id { get; set; }
        public int idPhieuMuon { get; set; }
        public int idNguoiDung { get; set; }
        public string maSach { get; set; }
        public int soLuong { get; set; }
        public string tinhTrang { get; set; }
        public string ghiChuNhanVien { get; set; }
        public float tienViPham { get; set; }
        public DateTime ngayMuon { get; set; }
        public DateTime ngayTra { get; set; }
    }
}
