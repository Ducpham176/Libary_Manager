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
    class BUS_QuanLyDocGia
    {
        private DAO_QuanLyDocGia quanLyDocGiaDAO;

        public BUS_QuanLyDocGia()
        {
            this.quanLyDocGiaDAO = new DAO_QuanLyDocGia();
        }

        public bool insertDocGia(DTO_QuanLyNguoiDung quanLyDocGiaDTO)
        {
            try
            {
                return quanLyDocGiaDAO.insertDocGia(quanLyDocGiaDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm độc giả: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable getDsDocGia()
        {
            try
            {
                return quanLyDocGiaDAO.getDsDocGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm độc giả: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool updateDocGia(DTO_QuanLyNguoiDung quanLyDocGiaDTO)
        {
            try
            {
                return quanLyDocGiaDAO.updateDocGia(quanLyDocGiaDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm độc giả: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
