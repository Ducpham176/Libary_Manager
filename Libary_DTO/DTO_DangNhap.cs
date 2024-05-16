using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    class DTO_DangNhap
    {
        public int id { get; set; }
        public string taiKhoan { get; set; }
        public string matKhau { get; set; }
        public string hoTen { get; set; }
        public int quyen { get; set; }
        public string email { get; set; }
        public string mssv { get; set; }
        public string gioiTinh { get; set; }
        public string diaChi { get; set; }
        public DateTime ngaySinh { get; set; }
        public DateTime ngayTao { get; set; }
    }
}
