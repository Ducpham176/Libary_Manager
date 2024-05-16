using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_DoiMatKhau
    {
        public bool updateMatKhau(DTO_QuanLyNguoiDung quanLyNguoiDungDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "matKhau", quanLyNguoiDungDTO.matKhau },
                };
                string condition = " id = '" + DTO_QuanLyNguoiDung.id + "'";
                return Database.update("TV_NguoiDung", data, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
