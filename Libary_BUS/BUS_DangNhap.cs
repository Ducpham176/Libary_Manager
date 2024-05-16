using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS
{
    internal class BUS_DangNhap
    {
        private DAO_DangNhap dangNhapDAO;

        public BUS_DangNhap()
        {
            this.dangNhapDAO = new DAO_DangNhap();
        }

        public DataTable checkDangNhap(DTO_DangNhap dangNhapDTO)
        {
            try
            {
                return dangNhapDAO.checkDangNhap(dangNhapDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng nhập: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool checkTrangThaiNhanVien(DTO_DangNhap dangNhapDTO)
        {
            try
            {
                return dangNhapDAO.checkTrangThaiNhanVien(dangNhapDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng nhập: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
