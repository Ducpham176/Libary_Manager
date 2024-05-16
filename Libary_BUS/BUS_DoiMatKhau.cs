using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class BUS_DoiMatKhau
    {
        private DAO_DoiMatKhau doiMatKhauDAO;

        public BUS_DoiMatKhau()
        {
            this.doiMatKhauDAO = new DAO_DoiMatKhau();
        }

        public bool updateMatKhau(DTO_QuanLyNguoiDung quanLyNguoiDungDTO)
        {
            try
            {
                return doiMatKhauDAO.updateMatKhau(quanLyNguoiDungDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đổi mật khẩu: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
