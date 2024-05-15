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
    class BUS_QuanLyNhanVien
    {
        private DAO_QuanLyNhanVien quanLyNhanVienDAO;

        public BUS_QuanLyNhanVien()
        {
            this.quanLyNhanVienDAO = new DAO_QuanLyNhanVien();
        }    
    
        public bool insertNhanVien(DTO_QuanLyNguoiDung quanLyNhanVienDTO)
        {
            try
            {
                return quanLyNhanVienDAO.insertNhanVien(quanLyNhanVienDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm nhân viên: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable getDsNhanVien()
        {
            try
            {
                return quanLyNhanVienDAO.getDsNhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm nhân viên: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }    

        public bool updateNhanVien(DTO_QuanLyNguoiDung quanLyNguoiDungDTO)
        {
            try
            {
                return quanLyNhanVienDAO.updateNhanVien(quanLyNguoiDungDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm nhân viên: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
