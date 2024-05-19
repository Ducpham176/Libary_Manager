using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_ChiTietPhieuMuon
    {
        public bool insertChiTietPhieuMuon(DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO)
        {
            try
            {
                // Tạo chi tiết phiếu mượn 
                var data = new Dictionary<string, object>()
                {
                    { "idPhieuMuon", chiTietPhieuMuonDTO.idPhieuMuon },
                    { "maSach", chiTietPhieuMuonDTO.maSach },
                    { "soLuong", chiTietPhieuMuonDTO.soLuong },
                    { "tinhTrang", chiTietPhieuMuonDTO.tinhTrang },
                };
                return Database.insert("TV_ChiTietPhieuMuon", data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
