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
                var data = new Dictionary<string, object>()
                {
                    { "idNguoiMuon", phieuMuonDTO.idNguoiMuon },
                    { "maSach", phieuMuonDTO.maSach },
                    { "tinhTrang", phieuMuonDTO.tinhTrang },
                    { "ngayMuon", DateTime.Now },
                };
                if (phieuMuonDTO.ghiChuDocGia != null)
                {
                    data.Add("ghiChuDocGia", phieuMuonDTO.ghiChuDocGia);
                }    
 
                Database.insert("TV_PhieuMuon", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
