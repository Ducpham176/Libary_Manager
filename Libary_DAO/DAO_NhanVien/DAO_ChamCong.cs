using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Libary_Manager.Libary_DAO.DAO_NhanVien
{
    class DAO_ChamCong
    {
        public DataTable getDanhSachNhanVienTruc()
        {
            try
            {
                string thu = "";
                switch (DTO_ChamCong.thuMay)
                {
                    case 1:
                        thu = "idChuNhat";
                        break;

                    case 2:
                        thu = "idThu2";
                        break;

                    case 3:
                        thu = "idThu3";
                        break;

                    case 4:
                        thu = "idThu4";
                        break;

                    case 5:
                        thu = "idThu5";
                        break;

                    case 6:
                        thu = "idThu6";
                        break;

                    case 7:
                        thu = "idThu7";
                        break;

                    default: break;
                }
                string sql = "SELECT " + thu + " FROM TV_PhanCongNhanVien WHERE maChiNhanh = '" + DTO_DangNhap.maChiNhanh + "'";
                
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        public DataTable getDsChamCong(DTO_ChamCong chamCongDTO, DateTime presentTime)
        {
            try
            {
                string sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS stt, " +
                    "TRIM(cc.caTruc) as caTruc, cc.moTaSuco, cc.viPham, cc.tgBatDau, cn.chiNhanh, cn.diaChi " +
                    "FROM TV_ChamCong cc JOIN TV_ChiNhanh cn ON cc.maChiNhanh = cn.id " +
                    "WHERE cc.idNhanVien = '" + chamCongDTO.idNhanVien + "' AND MONTH(tgBatDau) = MONTH(GETDATE()) ORDER BY cc.id DESC";
                
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getLichTrucTrongTuan(DTO_ChamCong chamCongDTO)
        {
            try
            {
                string sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS stt, " +
                    "cc.caTruc, cc.moTaSuco, cc.viPham, cc.tgBatDau, cn.chiNhanh, cn.diaChi " +
                    "FROM TV_ChamCong cc JOIN TV_ChiNhanh cn ON cc.maChiNhanh = cn.id " +
                    "WHERE cc.idNhanVien = '" + chamCongDTO.idNhanVien + "' AND MONTH(tgBatDau) = MONTH(GETDATE())";
                
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool createChamCong(DTO_ChamCong chamCongDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "idNhanVien", chamCongDTO.idNhanVien },
                    { "tgBatDau", chamCongDTO.tgBatDau },
                    { "caTruc", chamCongDTO.caTruc },
                    { "maChiNhanh", chamCongDTO.maChiNhanh },
                    { "moTaSuco", chamCongDTO.moTaSuco },
                    { "viPham", chamCongDTO.viPham },
                };  

                Database.insert("TV_ChamCong", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }   
        

        public bool checkTrangThaiChamCong(DTO_ChamCong chamCongDTO, int idCaTruc, string caTruc)
        {
            try
            {
                if (DTO_DangNhap.id == idCaTruc)
                {
                    string sql = "SELECT tgBatDau FROM TV_ChamCong WHERE idNhanVien = '" + DTO_DangNhap.id + "' " +
                        "AND DATEPART(DAY, GETDATE()) = DATEPART(DAY, tgBatDau) AND caTruc = N'" + caTruc + "'";
                    DataTable data = Database.read(sql);

                    if (data.Rows.Count > 0)
                    {
                        // Đã tồn tại
                        return false;
                    } else
                    {
                        this.createChamCong(chamCongDTO);
                        return true;
                    } 
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }


        public DataTable getDsLichTrucTrongTuan()
        {
            try
            {
                string sql = "SELECT " +
                "CASE WHEN idThu2 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu2 ELSE NULL END as Thu_2, " +
                "CASE WHEN idThu3 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu3 ELSE NULL END as Thu_3, " +
                "CASE WHEN idThu4 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu4 ELSE NULL END as Thu_4, " +
                "CASE WHEN idThu5 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu5 ELSE NULL END as Thu_5, " +
                "CASE WHEN idThu6 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu6 ELSE NULL END as Thu_6, " +
                "CASE WHEN idThu7 LIKE '%" + DTO_DangNhap.id + "%' THEN idThu7 ELSE NULL END as Thu_7, " +
                "CASE WHEN idChunhat LIKE '%" + DTO_DangNhap.id + "%' THEN idChunhat ELSE NULL END as ChuNhat " +
                "FROM TV_PhanCongNhanVien WHERE maChiNhanh = '" + DTO_DangNhap.maChiNhanh + "'";
   
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }
    }
}
