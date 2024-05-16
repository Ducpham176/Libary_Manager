using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DTO
{
    class DTO_ChamCong
    {
        public int id { get; set; }
        public int idNhanVien { get; set; }
        public DateTime tgBatDau { get; set; }
        public string caTruc { get; set; }
        public int maChiNhanh { get; set; }
        public string moTaSuco { get; set; }
        public string viPham { get; set; }

        // more 
        public static int thuMay { get; set; }
    }
}
