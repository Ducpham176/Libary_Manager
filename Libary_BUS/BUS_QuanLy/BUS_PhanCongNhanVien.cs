using Libary_Manager.Libary_DAO.DAO_QuanLy;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS.BUS_QuanLy
{
    class BUS_PhanCongNhanVien
    {
        private DAO_PhanCongNhanVien phanCongNhanVienDAO;


        public BUS_PhanCongNhanVien()
        {
            this.phanCongNhanVienDAO = new DAO_PhanCongNhanVien();
        }    

        public bool insertPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                return phanCongNhanVienDAO.insertPhanCongNhanVien(phanCongNhanVienDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể phân công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
