using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using Libary_Manager.Libary_DTO;


namespace Libary_Manager.Libary_DAO
{
    class DAO_DangNhap {
        public DataTable checkDangNhap(DTO_DangNhap dangNhapDTO)
        {
            try
            {
                string sql = "SELECT * FROM TV_NguoiDung " +
                    "WHERE taiKhoan ='" + dangNhapDTO.taiKhoan + "' AND " +
                    "matKhau = '" + Core.MD5Hash(dangNhapDTO.matKhau) + "'";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
