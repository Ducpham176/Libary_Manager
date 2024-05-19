using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace Libary_Manager.Libary_BUS
{
    class BUS_ChiTietPhieuMuon
    {
        private DAO_ChiTietPhieuMuon chiTietPhieuMuonDAO;


        public BUS_ChiTietPhieuMuon()
        {
            this.chiTietPhieuMuonDAO = new DAO_ChiTietPhieuMuon();
        }

        public bool insertChiTietPhieuMuon(DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO)
        {
            try
            {
                return chiTietPhieuMuonDAO.insertChiTietPhieuMuon(chiTietPhieuMuonDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm chi tiết phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
