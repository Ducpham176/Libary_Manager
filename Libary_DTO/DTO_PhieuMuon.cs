using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    class DTO_PhieuMuon
    {
        public int id { get; set; }
        public int idNguoiMuon { get; set; }
        public int idNhanVien { get; set; }
        public string maSach { get; set; }
        public string soLuong { get; set; }
        public string tinhTrang { get; set; }
        public string ghiChuDocGia { get; set; }
        public string ghiChuNhanVien { get; set; }
        public float tienViPham { get; set; }
        public DateTime ngayMuon { get; set; }
        public DateTime ngayTra { get; set; }

    }
}
