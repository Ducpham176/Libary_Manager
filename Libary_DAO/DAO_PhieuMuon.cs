using CLR;
using Guna.UI2.AnimatorNS;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_PhieuMuon
    {
        public DataTable getInfoPhieuSach(string condition, string orderby)
        {
            try
            {
                string sql = "SELECT TRIM(maSach) as pmMaSach, TRIM(tuaSach) as pmTuaSach, " +
                    "TRIM(photo) as pmPhoto, TRIM(cn.chiNhanh) as pmChiNhanh, TRIM(cn.diaChi) as pmDiaChi " +
                    "FROM TV_ChiNhanh cn INNER JOIN TV_Sach ON TV_Sach.maChiNhanh = cn.id " +
                    "WHERE maSach IN " + condition + " ORDER BY CASE maSach " +
                    orderby + " END;";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool insertPhieuMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                string sqlCheck = "SELECT * FROM TV_PhieuMuon " +
                    "WHERE idNguoiMuon = '" + phieuMuonDTO.idNguoiMuon + "' AND tinhTrang != N'Đã trả sách'";
                if (Database.read(sqlCheck).Rows.Count > 0)
                {
                    return false;
                }    
                else
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "idNguoiMuon", phieuMuonDTO.idNguoiMuon },
                        { "maSach", phieuMuonDTO.maSach },
                        { "soLuong", phieuMuonDTO.soLuong },
                        { "tinhTrang", phieuMuonDTO.tinhTrang },
                        { "ngayMuon", DateTime.Now },
                    };
                    return Database.insert("TV_PhieuMuon", data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable getDsYeuCauMuonSach()
        {
            try
            {
                string sql = "WITH OrderedBooks AS (\r\n    SELECT \r\n\t\tpm.id,\r\n        nd.hoTen,\r\n        s.tuaSach,\r\n\t\tREPLACE(pm.maSach, '|', '\n') as maSach,\r\n\t\tREPLACE(pm.soLuong, '|', '\n') as soLuong,\r\n        pm.ngayMuon,\r\n        pm.tinhTrang,\r\n        CHARINDEX(s.maSach, pm.maSach) AS OrderIndex\r\n    FROM TV_PhieuMuon pm\r\n    JOIN TV_NguoiDung nd ON nd.id = pm.idNguoiMuon\r\n    JOIN TV_Sach s ON pm.maSach COLLATE Latin1_General_CI_AI LIKE '%' + s.maSach + '%'\r\n    WHERE pm.tinhTrang = N'Phê duyệt'\r\n)\r\nSELECT \r\n    id,\r\n    hoTen,\r\n    STRING_AGG(tuaSach, '\n') WITHIN GROUP (ORDER BY OrderIndex) AS tuaSach,\r\n    REPLACE(maSach, '|', '\n') AS maSach,\r\n    REPLACE(soLuong, '|', '\n') AS soLuong,\r\n    ngayMuon\r\nFROM OrderedBooks\r\nGROUP BY id, hoTen, maSach, soLuong, ngayMuon;\r\n";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void chapNhanYeuCauMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "idNhanVien", phieuMuonDTO.idNhanVien },
                    { "tinhTrang", phieuMuonDTO.tinhTrang },
                    { "ngayBatDauMuon", phieuMuonDTO.ngayBatDauMuon },
                    { "ngayTra", phieuMuonDTO.ngayTra },
                };
                string condition = " id = '" + phieuMuonDTO.id + "'";

                Database.update("TV_PhieuMuon", data, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
