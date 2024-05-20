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

        public void chapNhanYeuCauMuon(DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO, DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "tinhTrang", chiTietPhieuMuonDTO.tinhTrang },
                    { "ngayMuon", chiTietPhieuMuonDTO.ngayMuon },
                    { "ngayTra", chiTietPhieuMuonDTO.ngayTra },
                };
                string condition = " idPhieuMuon = '" + phieuMuonDTO.id + "'";

                Database.update("TV_ChiTietPhieuMuon", data, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void tuChoiYeuCauMuon(DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO, DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "tinhTrang", chiTietPhieuMuonDTO.tinhTrang },
                };
                string condition = " idPhieuMuon = '" + phieuMuonDTO.id + "'";

                Database.update("TV_ChiTietPhieuMuon", data, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
