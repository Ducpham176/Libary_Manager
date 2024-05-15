using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    class DTO_Sach
    {
        public int id { get; set; }
        public string maSach { get; set; }
        public string tuaSach { get; set; }
        public string photo { get; set; }
        public string tacGia { get; set; }
        public string nhaXuatBan { get; set; }
        public string namXuatBan { get; set; }
        public int maChiNhanh { get; set; }
        public string loiGioiThieu { get; set; }
        public int soLuong { get; set; }
        public DateTime ngayThem { get; set; }
    }
}
