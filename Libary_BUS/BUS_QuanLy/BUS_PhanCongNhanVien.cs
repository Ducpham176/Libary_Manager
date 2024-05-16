using Libary_Manager.Libary_DAO.DAO_QuanLy;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
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

        public bool savedPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                return phanCongNhanVienDAO.savedPhanCongNhanVien(phanCongNhanVienDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể phân công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable dataPhieuPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                return phanCongNhanVienDAO.dataPhieuPhanCongNhanVien(phanCongNhanVienDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu phân công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getThongTinTheoThu(DTO_PhanCongNhanVien phanCongNhanVienDTO, string thu)
        {
            try
            {
                string idNhanVien = phanCongNhanVienDAO.getThongTinTheoThu(phanCongNhanVienDTO, thu);
                if (idNhanVien != "")
                {
                    string[] arrIds = idNhanVien.Split('|');
                    return phanCongNhanVienDAO.getThongTinNhanVienTheoThu(arrIds[0], arrIds[1]);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông tin phân công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }    
    }
}
