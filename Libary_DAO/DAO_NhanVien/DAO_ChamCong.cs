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
        public DataTable getDanhSachNhanVienTruc(DTO_ChamCong chamCongDTO)
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

                Database.insert("TV_ChamCong", data);
                MessageBox.Show("Saved");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }   
        

        public bool checkTrangThaiChamCong(DTO_ChamCong chamCongDTO, int idCaTruc)
        {
            try
            {
                if (DTO_DangNhap.id == idCaTruc)
                {
                    string sql = "SELECT tgBatDau FROM TV_ChamCong WHERE id = '" + DTO_DangNhap.id + "'";
                    DataTable data = Database.read(sql);

                    if (data.Rows.Count > 0)
                    {
                        // Đã tồn tại 
                        return false;
                    } 
                    else
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
    }
}
