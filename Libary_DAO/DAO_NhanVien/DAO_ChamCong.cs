using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO.DAO_NhanVien
{
    class DAO_ChamCong
    {
        public bool checkChamCongBuoiHomNay(DTO_ChamCong chamCongDTO)
        {
            try
            {
                string sql = "";
                return Database.read(sql).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
