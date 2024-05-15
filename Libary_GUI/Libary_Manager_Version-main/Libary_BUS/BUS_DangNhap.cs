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

        public bool checkDangNhap(DTO_DangNhap dangNhapDTO)
        {
            try
            {
                DataTable data = dangNhapDAO.checkDangNhap(dangNhapDTO);
                return data != null;
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Controller.isAlert("Thông tin tài khoản hoặc mật khẩu không chính xác" + ex.Message, "Lỗi", System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
