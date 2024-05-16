using Libary_Manager.Libary_DAO.DAO_NhanVien;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS.BUS_NhanVien
{
    class BUS_ChamCong
    {
        private DAO_ChamCong chamCongDAO; 

        public BUS_ChamCong()
        {
            this.chamCongDAO = new DAO_ChamCong();
        }

        public bool checkChamCongBuoiHomNay(DTO_ChamCong chamCongDTO)
        {
            try
            {
                return chamCongDAO.checkChamCongBuoiHomNay(chamCongDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
