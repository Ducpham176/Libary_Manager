using Guna.UI2.AnimatorNS;
using Libary_Manager.Libary_DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            string condition = null;
            bool isFirstCondition = true; 
            foreach (var book in dictioLoanSlip)
            {
                if (!isFirstCondition)
                {
                    condition += " OR ";
                }
                condition += "maSach = '" + book.Key + "' ";
                isFirstCondition = false;
            }
            return phieuMuonDAO.getInfoPhieuSach(condition);
        }
    }
}