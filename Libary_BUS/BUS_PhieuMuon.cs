using Guna.UI2.AnimatorNS;
using Guna.UI2.WinForms;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS
{
    class BUS_PhieuMuon
    {
        private DAO_PhieuMuon phieuMuonDAO;

        public static Dictionary<string, int> dictioLoanSlip = new Dictionary<string, int>();

        public static int totalPresent = 0;

        public static int valueIdPhieuMuon = -1;

        public BUS_PhieuMuon()
        {
            this.phieuMuonDAO = new DAO_PhieuMuon();
        }

        public DataTable getThongTinChuanBiPhieuSach()
        {
            string condition = " (";
            bool first = true; 
            foreach (var item in dictioLoanSlip)
            {
                if (!first)
                {
                    condition += ", "; 
                }
                else
                {
                    first = false;
                }

                condition += "'" + item.Key + "'"; 
            }
            condition += ") ";

            string orderby = "";
            int index = 1;
            foreach (var item in dictioLoanSlip)
            {
                orderby += " WHEN '" + item.Key + "' THEN " + index + " ";
                ++index;
            }

            return phieuMuonDAO.getThongTinChuanBiPhieuSach(condition, orderby);
        }

        public DataTable getThongTinPhieuMuon()
        {
            try
            {
                return phieuMuonDAO.getThongTinPhieuMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy thông tin phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable getYeuCauPhieuMuon()
        {
            try
            {
                return phieuMuonDAO.getYeuCauPhieuMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy thông tin yêu cầu phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public int phieuMuonQuaHan()
        {
            try
            {
                return phieuMuonDAO.phieuMuonQuaHan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được sách quá hạn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            };
        }

        public int soLuongPhieuMuon()
        {
            try
            {
                return phieuMuonDAO.soLuongPhieuMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được số lượng phiếu đã mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 6;
            };
        }

        public int soLuongPhieuDuocMuon()
        {
            try
            {
                return phieuMuonDAO.soLuongPhieuDuocMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được số lượng phiếu được mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            };
        }

        public bool createPhieuMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                return phieuMuonDAO.createPhieuMuon(phieuMuonDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể gửi yêu cầu phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }

        public int getIdPhieuMuon()
        {
            try
            {
                return phieuMuonDAO.getIdPhieuMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy id phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            };
        }

        public DataTable getDsYeuCauMuonSach()
        {
            try
            {
                return phieuMuonDAO.getDsYeuCauMuonSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy yêu cầu phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public void capNhatPhieuMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                phieuMuonDAO.capNhatPhieuMuon(phieuMuonDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chấp nhận phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        public DataTable getAllDsDaMuon()
        {
            try
            {
                return phieuMuonDAO.getAllDsDaMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy danh sách đã mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }
    }
}