using Libary_Manager.Libary_DAO.DAO_NhanVien;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS.BUS_NhanVien
{
    class BUS_ChamCong
    {
        private DAO_ChamCong chamCongDAO; 

        public BUS_ChamCong()
        {
            this.chamCongDAO = new DAO_ChamCong();
        }

        public bool checkChamCongBuoiHomNay(DTO_ChamCong chamCongDTO)
        {
            try
            {
                DataTable dsNhanVienToday = chamCongDAO.getDanhSachNhanVienTruc(chamCongDTO);
                if (dsNhanVienToday.Rows.Count > 0)
                {
                    string[] pathIDNhanViens = dsNhanVienToday.Rows[0][0].ToString().Split('|');
                    int hourRealTime = DateTime.Now.Hour;

                    if (hourRealTime < 11)
                    {
                        // Kiểm tra nếu pathIDNhanViens[0] không rỗng
                        if (!string.IsNullOrEmpty(pathIDNhanViens[0]))
                        {
                            int idCaTrucST = int.Parse(pathIDNhanViens[0]);
                            return chamCongDAO.checkTrangThaiChamCong(chamCongDTO, idCaTrucST);
                        }
                    }

                    else
                    {
                        // Check ca trua chiều 
                        if (!string.IsNullOrEmpty(pathIDNhanViens[1]))
                        {
                            int idCaTrucTC = int.Parse(pathIDNhanViens[1].ToString());
                            return chamCongDAO.checkTrangThaiChamCong(chamCongDTO, idCaTrucTC);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chấm công: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
