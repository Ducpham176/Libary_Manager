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
        public int idChiNhanh { get; set; }
        public DateTime ngayLapPhieu { get; set; }
    }
}
