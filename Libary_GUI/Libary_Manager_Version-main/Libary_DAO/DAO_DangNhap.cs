using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Libary_Manager.Libary_DTO;

namespace Libary_Manager.Libary_DAO
{
    internal class DAO_DangNhap
    {
    public DataTable checkDangNhap(DTO_DangNhap dangNhapDTO)
        {
            String sql = "SELECT id FROM TV_NguoiDung WHERE taiKhoan ='" + dangNhapDTO.taiKhoan + "' AND matKhau ='" + dangNhapDTO.matKhau + "'";
            return Database.read(sql);
        }
    }
}
