using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO.DAO_QuanLy
{
    class DAO_PhanCongNhanVien
    {
        public bool insertPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "maChiNhanh", phanCongNhanVienDTO.maChiNhanh },
                    { "idThu2", phanCongNhanVienDTO.idThu2 },
                    { "idThu3", phanCongNhanVienDTO.idThu3 },
                    { "idThu4", phanCongNhanVienDTO.idThu4 },
                    { "idThu5", phanCongNhanVienDTO.idThu5 },
                    { "idThu6", phanCongNhanVienDTO.idThu6 },
                    { "idThu7", phanCongNhanVienDTO.idThu7 },
                    { "idChuNhat", phanCongNhanVienDTO.idChuNhat },
                    { "thoiGian", DateTime.Now },
                };
                Database.insert("TV_PhanCongNhanVien", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool updatePhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "maChiNhanh", phanCongNhanVienDTO.maChiNhanh },
                    { "idThu2", phanCongNhanVienDTO.idThu2 },
                    { "idThu3", phanCongNhanVienDTO.idThu3 },
                    { "idThu4", phanCongNhanVienDTO.idThu4 },
                    { "idThu5", phanCongNhanVienDTO.idThu5 },
                    { "idThu6", phanCongNhanVienDTO.idThu6 },
                    { "idThu7", phanCongNhanVienDTO.idThu7 },
                    { "idChuNhat", phanCongNhanVienDTO.idChuNhat },
                };
                Database.insert("TV_PhanCongNhanVien", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
