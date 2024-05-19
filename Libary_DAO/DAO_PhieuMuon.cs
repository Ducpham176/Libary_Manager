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
        public DataTable getThongTinChuanBiPhieuSach(string condition, string orderby)
        {
            try
            {
                string sql = "SELECT TRIM(maSach) as pmMaSach, TRIM(tuaSach) as pmTuaSach, " +
                    "TRIM(photo) as pmPhoto, cn.id as pmId, TRIM(cn.chiNhanh) as pmChiNhanh, TRIM(cn.diaChi) as pmDiaChi " +
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

        public bool createPhieuMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            // Tạo phiếu mượn 
            var data = new Dictionary<string, object>()
            {
                { "idNguoiMuon", phieuMuonDTO.idNguoiMuon },
                { "idChiNhanh", phieuMuonDTO.idChiNhanh },
                { "ngayLapPhieu", phieuMuonDTO.ngayLapPhieu },
            };
            return Database.insert("TV_PhieuMuon", data);
        }

        public DataTable getThongTinPhieuMuon()
        {
            try
            {
                string sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS stt, \r\n       STRING_AGG(s.maSach, '\n') AS maSach, \r\n\t   STRING_AGG(ctpm.tinhTrang, '\n') as tinhTrang,\r\n       STRING_AGG(s.tuaSach, '\n') AS tuaSach, \r\n       STRING_AGG(ctpm.soLuong, '\n') as soLuong,\r\n\t   ctpm.ngayMuon,\r\n       STRING_AGG(ctpm.ngayTra, '\n') as soLuong\r\nFROM TV_NguoiDung nd\r\nJOIN TV_PhieuMuon pm ON pm.idNguoiMuon = nd.id\r\nJOIN TV_ChiTietPhieuMuon ctpm ON ctpm.idPhieuMuon = pm.id\r\nJOIN TV_Sach s ON s.maSach = ctpm.maSach\r\nWHERE nd.id = '" + DTO_DangNhap.id + "' \r\nGROUP BY ctpm.ngayMuon, ctpm.tinhTrang";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getYeuCauPhieuMuon()
        {
            try
            {
                string sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS stt,\r\n       pm.id,\r\n       nd.hoTen,\r\n       STRING_AGG(s.maSach, '\n') AS maSach, \r\n       STRING_AGG(s.tuaSach, '\n') AS tuaSach, \r\n       STRING_AGG(CAST(ctpm.soLuong AS VARCHAR), '\n') as soLuong,\r\n       pm.ngayLapPhieu,\r\n       STRING_AGG(CAST(ctpm.ngayTra AS VARCHAR), '\n') as ngayTra\r\nFROM TV_NguoiDung nd\r\nJOIN TV_PhieuMuon pm ON pm.idNguoiMuon = nd.id\r\nJOIN TV_ChiTietPhieuMuon ctpm ON ctpm.idPhieuMuon = pm.id\r\nJOIN TV_Sach s ON s.maSach = ctpm.maSach\r\nWHERE pm.idChiNhanh = '" + DTO_DangNhap.maChiNhanh + "' GROUP BY pm.id, nd.hoTen, pm.ngayLapPhieu, ctpm.tinhTrang\r\n";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public int getIdPhieuMuon()
        {
            // Get Phiếu mượn vừa tạo 
            string sql = "SELECT id FROM TV_PhieuMuon " +
                "WHERE idNguoiMuon = '" + DTO_DangNhap.id + "' ORDER BY id DESC";
            int total;
            if (Database.read(sql).Rows.Count > 0)
            {
                total = int.Parse(Database.read(sql).Rows[0]["id"].ToString());
            }    
            else
            {
                total = 0;
            }

            return total;
        }    

        public int phieuMuonQuaHan()
        {
            string sql = "SELECT * FROM TV_PhieuMuon pm JOIN TV_ChiTietPhieuMuon ctpm ON pm.id = ctpm.idPhieuMuon " +
                "WHERE pm.idNguoiMuon = '" + DTO_DangNhap.id + "' AND tinhTrang = N'Quá hạn'";
            int total = Database.read(sql).Rows.Count;
            return total;
        }

        public int soLuongPhieuMuon()
        {
            string sql = "SELECT SUM(soLuong) AS SoLuong FROM TV_ChiTietPhieuMuon ctpm " +
                "JOIN TV_PhieuMuon pm ON ctpm.idPhieuMuon = pm.id " +
                "WHERE pm.idNguoiMuon = '" + DTO_DangNhap.id + "' AND (tinhTrang = N'Phê duyệt' OR tinhTrang = N'Đang mượn');";
            int total;
            if (Database.read(sql).Rows.Count > 0 
                && Database.read(sql).Rows[0]["soLuong"].ToString() != "")
            {
                total = int.Parse(Database.read(sql).Rows[0]["soLuong"].ToString());
            }    
            else
            {
                total = 0;
            }
            return total;
        }

        public int soLuongPhieuDuocMuon()
        {
            string sql = "SELECT slm.soLuongMuon FROM TV_NguoiDung nd " +
                       "JOIN TV_SoLuongMuon slm ON nd.quyen = slm.quyen WHERE nd.id = '" + DTO_DangNhap.id + "'";
            int total;
            if (Database.read(sql).Rows.Count > 0)
            {
                 total = int.Parse(Database.read(sql).Rows[0]["soLuongMuon"].ToString());
            }    
            else
            {
                total = 0;
            }
            return total;
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
            /*    var data = new Dictionary<string, object>()
                {
                    { "idNhanVien", phieuMuonDTO.idNhanVien },
                    { "tinhTrang", phieuMuonDTO.tinhTrang },
                    { "ngayBatDauMuon", phieuMuonDTO.ngayBatDauMuon },
                    { "ngayTra", phieuMuonDTO.ngayTra },
                };
                string condition = " id = '" + phieuMuonDTO.id + "'";

                Database.update("TV_PhieuMuon", data, condition);*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void huyBoYeuCauMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    /*{ "tinhTrang", phieuMuonDTO.tinhTrang },*/
                };
                string condition = " id = '" + phieuMuonDTO.id + "'";

                Database.update("TV_PhieuMuon", data, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       /* public bool checkStatusMuonSach(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                string sql = "SELECT TOP 1 pm.*, nd.*, s.*\r\nFROM TV_PhieuMuon pm\r\nJOIN TV_NguoiDung nd ON pm.idNguoiMuon = nd.id " +
                    "JOIN TV_Sach s ON pm.maSach LIKE N'%" + phieuMuonDTO.maSach + "%' " +
                    "WHERE nd.id = '" + DTO_DangNhap.id + "' AND (tinhTrang = N'Đang mượn' OR tinhTrang = N'Đã trả sách')\r\n";
                return Database.read(sql).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }*/
    }
}
