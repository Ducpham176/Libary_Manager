using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO.DAO_QuanLy
{
    class DAO_PhanCongNhanVien
    {
        public bool savedPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                string condition = " maChiNhanh = '" + phanCongNhanVienDTO.maChiNhanh + "'";
                string sql = "SELECT id FROM TV_PhanCongNhanVien WHERE" + condition;
                int total = Database.read(sql).Rows.Count;

                var data = new Dictionary<string, object>()
                {
                    { "idThu2", phanCongNhanVienDTO.idThu2 },
                    { "idThu3", phanCongNhanVienDTO.idThu3 },
                    { "idThu4", phanCongNhanVienDTO.idThu4 },
                    { "idThu5", phanCongNhanVienDTO.idThu5 },
                    { "idThu6", phanCongNhanVienDTO.idThu6 },
                    { "idThu7", phanCongNhanVienDTO.idThu7 },
                    { "idChuNhat", phanCongNhanVienDTO.idChuNhat },
                };
                if (total > 0)
                {
                    Database.update("TV_PhanCongNhanVien", data, condition); return true;
                } 
                else
                {
                    data.Add("maChiNhanh", phanCongNhanVienDTO.maChiNhanh);
                    data.Add("thoiGian", DateTime.Now);
                    Database.insert("TV_PhanCongNhanVien", data); return true;
                }
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

        public DataTable dataPhieuPhanCongNhanVien(DTO_PhanCongNhanVien phanCongNhanVienDTO)
        {
            try
            {
                string sql = "SELECT * FROM TV_PhanCongNhanVien WHERE maChiNhanh = '" + phanCongNhanVienDTO.maChiNhanh + "'";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public string getThongTinTheoThu(DTO_PhanCongNhanVien phanCongNhanVienDTO, string thu)
        {
            try
            {
                string sql = "SELECT " + thu + " FROM TV_PhanCongNhanVien WHERE maChiNhanh = '" + phanCongNhanVienDTO.maChiNhanh + "'";
                return Database.read(sql).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getThongTinNhanVienTheoThu(string nvSang, string nvTrua)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ROW_NUMBER() OVER (ORDER BY ");

                if (!string.IsNullOrEmpty(nvSang))
                {
                    sql.Append("CASE WHEN id = " + nvSang + " THEN 1 ");
                }

                if (!string.IsNullOrEmpty(nvTrua))
                {
                    if (!string.IsNullOrEmpty(nvSang))
                    {
                        sql.Append("WHEN id = " + nvTrua + " THEN 2 ");
                    }
                    else
                    {
                        sql.Append("CASE WHEN id = " + nvTrua + " THEN 1 ");
                    }
                }

                sql.Append("END) as stt, hoTen, taiKhoan, email, gioiTinh, ngayTao FROM TV_NguoiDung WHERE id IN (");

                List<string> ids = new List<string>();
                if (!string.IsNullOrEmpty(nvSang))
                {
                    ids.Add(nvSang);
                }

                if (!string.IsNullOrEmpty(nvTrua))
                {
                    ids.Add(nvTrua);
                }

                sql.Append(string.Join(", ", ids));
                sql.Append(")");

                return Database.read(sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

    }
}
