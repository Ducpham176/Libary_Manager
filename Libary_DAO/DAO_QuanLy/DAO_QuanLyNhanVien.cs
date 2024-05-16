using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Helpers.FindSearchRichParser;

namespace Libary_Manager.Libary_DAO.DAO_QuanLy
{
    class DAO_QuanLyNhanVien
    {
        public DataTable getDsNhanVien()
        {
            try
            {
                string sql = "SELECT id, TRIM(hoTen) as hoTen, TRIM(taiKhoan) as taiKhoan, TRIM(email) as email, " +
                    "gioiTinh, CASE WHEN trangThai != -1 THEN N'Đang làm việc' ELSE N'Đã nghỉ làm' END as trangThai, " +
                    "TRIM(diaChi) as diaChi, ngaySinh, ngayTao FROM TV_NguoiDung WHERE quyen = 1 ORDER BY id DESC;";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool insertNhanVien(DTO_QuanLyNguoiDung quanLyNhanVienDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "taiKhoan", quanLyNhanVienDTO.taiKhoan },
                    { "matKhau", quanLyNhanVienDTO.matKhau },
                    { "hoTen", quanLyNhanVienDTO.hoTen },
                    { "quyen", quanLyNhanVienDTO.quyen },
                    { "trangThai", quanLyNhanVienDTO.trangThai },
                    { "email", quanLyNhanVienDTO.email },
                    { "gioiTinh", quanLyNhanVienDTO.gioiTinh },
                    { "diaChi", quanLyNhanVienDTO.diaChi },
                    { "ngaySinh", quanLyNhanVienDTO.ngaySinh },
                    { "ngayTao", DateTime.Now },
                };
                Database.insert("TV_NguoiDung", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool updateNhanVien(DTO_QuanLyNguoiDung quanLyNhanVienDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "hoTen", quanLyNhanVienDTO.hoTen },
                    { "quyen", quanLyNhanVienDTO.quyen },
                    { "trangThai", quanLyNhanVienDTO.trangThai },
                    { "gioiTinh", quanLyNhanVienDTO.gioiTinh },
                    { "diaChi", quanLyNhanVienDTO.diaChi },
                    { "ngaySinh", quanLyNhanVienDTO.ngaySinh },
                };
                string condition = " taiKhoan = '" + quanLyNhanVienDTO.taiKhoan + "'";
                Database.update("TV_NguoiDung", data, condition); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
