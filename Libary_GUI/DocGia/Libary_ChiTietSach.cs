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
using DevExpress.XtraBars.Docking2010.Base;
using DevExpress.Utils.VisualEffects;
using System.Text.RegularExpressions;
using DevExpress.Utils.CodedUISupport;

namespace Libary_Manager.Libary_GUI.DoGia
{
    public partial class Libary_ChiTietSach : Form
    {
        private BUS_Sach sachBUS;
        private BUS_PhieuMuon phieuMuonBUS;
        private BUS_ChiTietPhieuMuon chiTietPhieuMuonBUS;

        private DTO_PhieuMuon phieuMuonDTO;
        private DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO;
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
                LbIdChiNhanh.Text = data.Rows[0]["id"].ToString();
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

        private void checkStatusComment()
        {
/*            if (phieuMuonBUS.checkStatusMuonSach(phieuMuonDTO))
            {
                // kiểm tra và load dữ liệu commect 
            }
            else
            {
                PtAlertBinhLuan.Visible = true;
            }*/
        }

        private void Libary_ChiTietSach_Load(object sender, EventArgs e)
        {
            this.chiTietPhieuMuonBUS = new BUS_ChiTietPhieuMuon();

            this.phieuMuonDTO = new DTO_PhieuMuon();
            this.chiTietPhieuMuonDTO = new DTO_ChiTietPhieuMuon();

            LbTotalSach.Text = BUS_PhieuMuon.totalPresent.ToString();
            this.checkStatusComment();

            NeSoLuong.Maximum = (DTO_DangNhap.quyen == 2) ? 3 : 5;
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
                Controller.isAlert(MdChiTietSach, "Không hợp lệ", "Vui lòng chọn số sao", MessageDialogIcon.Error);
            } else
            {
                if (Controller.isLength(TbNoiDung.Text, 6))
                {
                    MessageBox.Show(starSelected.ToString());
                }
                else
                {
                    Controller.isAlert(MdChiTietSach, "Không hợp lệ", "Nội dung phải lớn hơn 4 kí tự!", MessageDialogIcon.Error);
                }
            } 
        }

        private void BtnMuonNgay_Click(object sender, EventArgs e)
        {
            if (phieuMuonBUS.phieuMuonQuaHan() > 0)
            {
                Controller.isAlert(MdChiTietSach, "Xảy ra lỗi", "Bạn đang có phiếu sách bị hết hạn", MessageDialogIcon.Error);
            }
            else
            {
                int phieuDaMuon = phieuMuonBUS.soLuongPhieuMuon();
                int phieuDuocMuon = phieuMuonBUS.soLuongPhieuDuocMuon();
                if ((phieuDaMuon + 1) > phieuDuocMuon)
                {
                    Controller.isAlert(MdChiTietSach, "Xảy ra lỗi", "Số sách được phép mượn, vượt quá hạn mức", MessageDialogIcon.Error);
                }
                else
                {
                    phieuMuonDTO.idNguoiMuon = DTO_DangNhap.id;
                    phieuMuonDTO.idChiNhanh = int.Parse(LbIdChiNhanh.Text);
                    phieuMuonDTO.ngayLapPhieu = DateTime.Now;
                    phieuMuonBUS.createPhieuMuon(phieuMuonDTO);

                    // Lưu chi tiết phiếu mượn
                    chiTietPhieuMuonDTO.idPhieuMuon = phieuMuonBUS.getIdPhieuMuon();
                    chiTietPhieuMuonDTO.maSach = LbMaSach.Text;
                    chiTietPhieuMuonDTO.soLuong = 1;
                    chiTietPhieuMuonDTO.tinhTrang = "Phê duyệt";
                    if (chiTietPhieuMuonBUS.insertChiTietPhieuMuon(chiTietPhieuMuonDTO))
                    {
                        Controller.isAlert(MdChiTietSach, "Thành công", "Phiếu mượn đã được gửi tới nhân viên", MessageDialogIcon.None);
                    }    
                }
            }
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
            if (phieuMuonBUS.phieuMuonQuaHan() > 0)
            {
                Controller.isAlert(MdChiTietSach, "Xảy ra lỗi", "Bạn đang có phiếu sách bị hết hạn", MessageDialogIcon.Error);
            }
            else
            {
                int phieuDaMuon = phieuMuonBUS.soLuongPhieuMuon();
                int phieuDuocMuon = phieuMuonBUS.soLuongPhieuDuocMuon();

                string maSach = LbMaSach.Text;
                int totalSach = int.Parse(NeSoLuong.Value.ToString());
                int maxSach = (DTO_DangNhap.quyen == 2) ? 3 : 5;

                if (BUS_PhieuMuon.totalPresent == 0)
                {
                    BUS_PhieuMuon.totalPresent = phieuDaMuon;
                }    

                if ((BUS_PhieuMuon.totalPresent + totalSach) > phieuDuocMuon)
                {
                    Controller.isAlert(MdChiTietSach, "Xảy ra lỗi", "Số sách được phép mượn, vượt quá hạn mức", MessageDialogIcon.Error);
                }
                else
                {
                    BUS_PhieuMuon.totalPresent += totalSach;

                    if (BUS_PhieuMuon.totalPresent == 0 && totalSach == maxSach)
                    {
                        checkExistSachToPhieuMuon(maSach, totalSach);
                        loadTabPhieuMuon();
                    }
                    else if (BUS_PhieuMuon.totalPresent > maxSach)
                    {
                        BUS_PhieuMuon.totalPresent -= totalSach;
                    }
                    else
                    {
                        checkExistSachToPhieuMuon(maSach, totalSach);
                        if (BUS_PhieuMuon.totalPresent == maxSach)
                        {
                            loadTabPhieuMuon();
                        }
                    }
                }
               
                LbTotalSach.Text = BUS_PhieuMuon.totalPresent.ToString();
            }
        }
    }
}
