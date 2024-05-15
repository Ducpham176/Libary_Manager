using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_PhieuMuon
    {
        public DataTable getInfoPhieuSach(string condition)
        {
            try
            {
                string sql = "SELECT TRIM(maSach) as pmMaSach, TRIM(tuaSach) as pmTuaSach, TRIM(photo) as pmPhoto, " +
                    "TRIM(cn.diaChi) as pmDiaChi, soluong as pmSoLuong " +
                    "FROM TV_ChiNhanh cn INNER JOIN TV_Sach ON TV_Sach.maChiNhanh = cn.id " +
                    "WHERE " + condition + " ORDER BY TV_Sach.id DESC";
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
