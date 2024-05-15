using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using Libary_Manager.Libary_GUI.DoGia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using Guna.UI2.WinForms;
using System.Text.RegularExpressions;

namespace Libary_Manager.Libary_GUI.DoGia
{
    public partial class Libary_ChiTietSach : Form
    {
        private BUS_Sach sachBUS;

        private BUS_PhieuMuon phieuMuonBUS;

        // Đánh giá sách

        private int starSelected = 0;

        // ................................................

        public Libary_ChiTietSach()
        {
            InitializeComponent();

            this.sachBUS = new BUS_Sach();
            this.phieuMuonBUS = new BUS_PhieuMuon();

            DataTable data = sachBUS.readSachEqualMaSach(sachBUS.getMaSach());
            if (data != null)
            {
                LbMaSach.Text = data.Rows[0]["maSach"].ToString();
                LbTuaSach.Text = data.Rows[0]["tuaSach"].ToString();
                LbSoLuong.Text = data.Rows[0]["soLuong"].ToString();
                LbChiNhanh.Text = data.Rows[0]["chiNhanh"].ToString();
                LbDiaChi.Text = data.Rows[0]["diaChi"].ToString();
                LbNhaXuatBan.Text = data.Rows[0]["nhaXuatBan"].ToString();
                LbNamXuatBan.Text = data.Rows[0]["namXuatBan"].ToString();
                LbTacGia.Text = data.Rows[0]["tacGia"].ToString();
                LbLoiGioiThieu.Text = data.Rows[0]["loiGioiThieu"].ToString();

                string pathImage = Controller._PATH_PHOTO_BOOK + data.Rows[0]["photo"].ToString();
                if (File.Exists(pathImage))
                {
                    PtAnhSach.Image = Image.FromFile(pathImage);
                }    
            }    
        }

        // ................................................

        private void Libary_ChiTietSach_Load(object sender, EventArgs e)
        {
            LbTotalSach.Text = BUS_PhieuMuon.totalPresent.ToString();
        }

        private void setFillImageRadio(params Guna2ImageRadioButton[] radios)
        {
            foreach (var radio in radios)
            {
                radio.Image = Image.FromFile(Controller._PATH_PHOTO_ITEM + "star_fill.png");
            }
        }

        private void noFillImageRadio(params Guna2ImageRadioButton[] radios)
        {
            foreach (var radio in radios)
            {
                radio.Image = Image.FromFile(Controller._PATH_PHOTO_ITEM + "star_nofill.png");
            }
        }

        private void Rstar1_CheckedChanged(object sender, EventArgs e)
        {
            starSelected = 1;
            noFillImageRadio(Rstar2, Rstar3, Rstar4, Rstar5);
        }

        private void Rstar2_CheckedChanged(object sender, EventArgs e)
        {
            starSelected = 2;
            setFillImageRadio(Rstar1);
            noFillImageRadio(Rstar3, Rstar4, Rstar5);
        }

        private void Rstar3_CheckedChanged(object sender, EventArgs e)
        {
            starSelected = 3;
            setFillImageRadio(Rstar1, Rstar2);
            noFillImageRadio(Rstar4, Rstar5);
        }

        private void Rstar4_CheckedChanged(object sender, EventArgs e)
        {
            starSelected = 4;
            setFillImageRadio(Rstar1, Rstar2, Rstar3);
            noFillImageRadio(Rstar5);
        }

        private void Rstar5_CheckedChanged(object sender, EventArgs e)
        {
            starSelected = 5;
            setFillImageRadio(Rstar1, Rstar2, Rstar3, Rstar4);
        }

        private void btnDongSachNay_Click(object sender, EventArgs e)
        {
            Libary_DocGia handle = new Libary_DocGia();

            string tabNameToRemove = "Sach" + LbMaSach.Text;

            foreach (TabPage tabPage in sachBUS.getTabControl().TabPages)
            {
                if (tabPage.Name == tabNameToRemove)
                {
                    sachBUS.getTabControl().TabPages.Remove(tabPage);
                    break;
                }
                if (tabPage.Name == "TabSachThuVien")
                {
                    sachBUS.getTabControl().SelectedTab = tabPage;
                }
            }
            sachBUS.getObjectSavedTabName().Remove(tabNameToRemove);
        }

        private void BtnDangBinhLuan_Click(object sender, EventArgs e)
        {
            if (starSelected == 0)
            {
                Controller.isAlert("Vui lòng chọn số sao", "Lỗi xảy ra", MessageBoxIcon.Error);
            } else
            {
                if (Controller.isLength(TbNoiDung.Text, 6))
                {
                    MessageBox.Show(starSelected.ToString());
                }
                else
                {
                    Controller.isAlert("Nội dung phải lớn hơn 4 kí tự!", "Lỗi xảy ra", MessageBoxIcon.Error);
                }
            } 
        }

        private void BtnMuonNgay_Click(object sender, EventArgs e)
        {

        }

        private void loadTabPhieuMuon()
        {
            foreach (TabPage tabPage in sachBUS.getTabControl().TabPages)
            {
                if (tabPage.Name == "TabPhieuMuon")
                {
                    sachBUS.getTabControl().SelectedTab = tabPage;
                }    
            }    
        }

        private void checkExistSachToPhieuMuon(string maSach, int total)
        {
            if (BUS_PhieuMuon.dictioLoanSlip.ContainsKey(maSach))
            {
                int currentValue;
                if (BUS_PhieuMuon.dictioLoanSlip.TryGetValue(maSach, out currentValue))
                {
                    BUS_PhieuMuon.dictioLoanSlip[maSach] = currentValue + total;
                }
            } 
            else
            {
                BUS_PhieuMuon.dictioLoanSlip.Add(maSach, total);
            }
        }

        private void BtnThemVaoPhieu_Click(object sender, EventArgs e)
        {
            string maSach = LbMaSach.Text;
            int totalSach = int.Parse(NeSoLuong.Value.ToString());
            BUS_PhieuMuon.totalPresent += totalSach;

            if (totalSach == 3)
            {
                checkExistSachToPhieuMuon(maSach, totalSach);
                loadTabPhieuMuon();
            }
            else if (BUS_PhieuMuon.totalPresent > 3)
            {
                BUS_PhieuMuon.totalPresent -= totalSach;
                Controller.isAlert("Số sách mượn trong 1 lần không được vượt quá 3 quyển", "Chú ý", MessageBoxIcon.Warning);
            }    
            else
            {
                checkExistSachToPhieuMuon(maSach, totalSach);
                if (BUS_PhieuMuon.totalPresent == 3)
                {
                    loadTabPhieuMuon();
                }    
            }
            LbTotalSach.Text = BUS_PhieuMuon.totalPresent.ToString();
        }


    }
}
