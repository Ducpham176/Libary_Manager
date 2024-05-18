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

        public BUS_PhieuMuon()
        {
            this.phieuMuonDAO = new DAO_PhieuMuon();
        }

        public DataTable getInfoPhieuSach()
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

            return phieuMuonDAO.getInfoPhieuSach(condition, orderby);
        }

        public bool insertPhieuMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                return phieuMuonDAO.insertPhieuMuon(phieuMuonDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể gửi yêu cầu phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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

        public void chapNhanYeuCauMuon(DTO_PhieuMuon phieuMuonDTO)
        {
            try
            {
                phieuMuonDAO.chapNhanYeuCauMuon(phieuMuonDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chấp nhận phiếu mượn: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }
    }
}