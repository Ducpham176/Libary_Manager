using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    internal class DTO_DangNhap
    {
        public int id { get; set; }
        public string taiKhoan { get; set; }
        public string matKhau { get; set; }
        public int quyen { get; set; }
        public DateTime ngayCap { get; set; }
        public int maThe { get; set; }
        public String hoTen { get; set; }
        public DateTime ngaySinh { get; set; }
        public String gioi { get; set; }
    }
}
