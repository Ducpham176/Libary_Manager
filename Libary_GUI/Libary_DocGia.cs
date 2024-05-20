using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars.Navigation;
using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using Libary_Manager.Libary_GUI;
using Libary_Manager.Libary_GUI.DoGia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Libary_Manager
{
    public partial class Libary_DocGia : Form
    {
        private void activedDoubleBuffered(Guna2DataGridView data)
        {
            typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            data,
            new object[] { true });
        }

        public Libary_DocGia()
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.activedDoubleBuffered(DgvSachThuVien);
            this.activedDoubleBuffered(DgvPhieuMuon);
            this.activedDoubleBuffered(DgvLichSuPhieuMuon);
        }


        private int _PAGE = 1;
        private ArrayList objectSavedTabName = new ArrayList();

        private BUS_Sach sachBUS;
        private BUS_PhieuMuon phieuMuonBUS;
        private BUS_DoiMatKhau doiMatKhauBUS;
        private BUS_ChiTietPhieuMuon chiTietPhieuMuonBUS;

        // ................................................

        private DTO_Sach sachDTO;
        private DTO_PhieuMuon phieuMuonDTO;
        private DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO;
        private DTO_QuanLyNguoiDung quanLyNguoiDungDTO;

        // ................................................

        private void Libary_TrangChu_Load(object sender, EventArgs e)
        {
            this.sachDTO = new DTO_Sach();
            this.phieuMuonDTO = new DTO_PhieuMuon();
            this.chiTietPhieuMuonDTO = new DTO_ChiTietPhieuMuon();
            this.quanLyNguoiDungDTO = new DTO_QuanLyNguoiDung();

            this.sachBUS = new BUS_Sach();
            this.doiMatKhauBUS = new BUS_DoiMatKhau();
            this.phieuMuonBUS = new BUS_PhieuMuon();
            this.chiTietPhieuMuonBUS = new BUS_ChiTietPhieuMuon();

            this.TabThongTinAction();
        }

        // ................................................

        // Chọn tab thông tin 
        void TabThongTinAction()
        {
            LbQuyen.Text = Controller.isRevertTypeQuyen();
            LbHoTen.Text = DTO_DangNhap.hoTen;
            LbGioiTinh.Text = DTO_DangNhap.gioiTinh;
            LbNgaySinh.Text = DTO_DangNhap.ngaySinh.ToString();
            LbNgayThamGia.Text = DTO_DangNhap.ngayTao.ToString();
            LbDiaChi.Text = DTO_DangNhap.diaChi;
        }

        // Chọn sách thư viện
        void TabSachThuVienAction()
        {
            sachBUS.setTabControl(TcLibary);

            // Load toàn bộ danh sách Sách
            if (!Controller.isEmpty(TbTuKhoa.Text))
            {
                DataTable data = sachBUS.dataPagination(_PAGE);
                Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
            }    
        }

        // Chọn đổi mật khẩu 
        void TabDoiMatKhauAction()
        {

        }

        // ................................................

        // Chọn chuẩn bị phiếu mượn
        void TabPhieuMuonAction()
        {
            if (BUS_PhieuMuon.dictioLoanSlip.Count > 0)
            {
                LbAlertNoData.Visible = false;
                DgvPhieuMuon.Visible = true;
                BtnGuiPhieuMuon.Visible = true;

                DataTable data = phieuMuonBUS.getThongTinChuanBiPhieuSach();
                Controller.isLoadDataPhoto(data, DgvPhieuMuon, "pmPhoto");

                int rowIndex = 0;
                foreach (var item in BUS_PhieuMuon.dictioLoanSlip)
                {
                    if (rowIndex < DgvPhieuMuon.Rows.Count)
                    {
                        DgvPhieuMuon.Rows[rowIndex].Cells["pmSoLuong"].Value = item.Value;
                    }
                    rowIndex++;
                }
                int desiredHeight = (BUS_PhieuMuon.dictioLoanSlip.Count + 1) * 100;
                DgvPhieuMuon.Height = desiredHeight;
            }
            else
            {
                LbAlertNoData.Visible = true;
                DgvPhieuMuon.Visible = false;
                BtnGuiPhieuMuon.Visible = false;
            }
        }

        // Chọn đăng xuất 
        private void TabDangXuatAction()
        {
            bool userConfirmed = Controller.isQuestion(MdDocGia, "Xác nhận hành động", "Bạn có chắc chắn muốn đăng xuất?");
            if (userConfirmed)
            {
                this.Close();
            }
            else
            {
                TcLibary.SelectedTab = TabPhieuDaMuon;
            }    
        }

        // ................................................

        // Load form tương thích
        private void TcLibary_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabPhieuDaMuon)
            {
                this.TabThongTinAction();
            }    

            if (selectedTabPage == TabSachThuVien)
            {
                this.TabSachThuVienAction();
            }

            if (selectedTabPage == TabDoiMatKhau)
            {
                this.TabDoiMatKhauAction();
            }

            if (selectedTabPage == TabPhieuMuon)
            {
                this.TabPhieuMuonAction();
            }

            if (selectedTabPage == TabDangXuat)
            {
                this.TabDangXuatAction();
            }
        }

        // Load form sách vào tabPage 
        private void isLoadFormToTabPage(Form form, TabPage tab, TabControl tabControl, DTO_Sach sachDTO)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowIcon = false;
            tab.Controls.Add(form);
            tab.Text = "Sách | " + sachDTO.tuaSach;
            tab.Name = "Sach" + sachDTO.maSach;
            tab.BackColor = Color.FromArgb(36, 37, 38);
            objectSavedTabName.Add(tab.Name);
            sachBUS.setObjectSavedTabName(objectSavedTabName);

            tabControl.TabPages.Insert(4, tab);
            tabControl.SelectedTab = tab;
            form.Show();
        }

        private void DgvSachThuVien_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvSachThuVien.Rows[e.RowIndex];

                // Lấy giá trị của cột "maSach" từ hàng được chọn
                sachDTO.maSach = row.Cells["maSach"].Value.ToString();
                sachDTO.tuaSach = row.Cells["tuaSach"].Value.ToString();

                if (objectSavedTabName.Contains("Sach" + sachDTO.maSach))
                {
                    TcLibary.SelectedTab = (TabPage)TcLibary.Controls["Sach" + sachDTO.maSach];
                }
                else
                {
                    // Lưu lại tên tabPage
                    sachBUS.setMaSach(sachDTO.maSach);
                    Libary_ChiTietSach form = new Libary_ChiTietSach();
                    TabPage tab = new TabPage();
                    isLoadFormToTabPage(form, tab, TcLibary, sachDTO);
                }
            }
        }

        private void BtnTiepTuc_Click(object sender, EventArgs e)
        {
            if (_PAGE < BUS_Sach._TOTAL_BOOK)
            {
                _PAGE += 1;
            }    
            else
            {
                _PAGE = BUS_Sach._TOTAL_BOOK;
            }
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnQuayLai_Click(object sender, EventArgs e)
        {
            if (_PAGE < 1)
            {
                _PAGE -= 1;
            }
            else
            {
                _PAGE = 1;
            }
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnTrangDau_Click(object sender, EventArgs e)
        {
            _PAGE = 1;
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnTrangCuoi_Click(object sender, EventArgs e)
        {
            _PAGE = Convert.ToInt32(Math.Ceiling((double)BUS_Sach._TOTAL_BOOK / Controller._MAX_PAGE));
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnDongTatCa_Click(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in TcLibary.TabPages)
            {
                if (objectSavedTabName.Contains(tabPage.Name))
                {
                    TcLibary.TabPages.Remove(tabPage);
                }    
            }
            objectSavedTabName.Clear();
        }

        private void DgvPhieuMuon_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvSachThuVien.Rows[e.RowIndex];
                object editValue = DgvPhieuMuon.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void BtnGuiPhieuMuon_Click(object sender, EventArgs e)
        {
            if (BUS_PhieuMuon.dictioLoanSlip.Count > 0)
            {
                if (BUS_PhieuMuon.valueIdPhieuMuon == -1)
                {
                    BUS_PhieuMuon.valueIdPhieuMuon = phieuMuonBUS.getIdPhieuMuon();
                }    
                phieuMuonDTO.idNguoiMuon = DTO_DangNhap.id;
                phieuMuonDTO.idChiNhanh = int.Parse(DgvPhieuMuon.Rows[0].Cells["pmId"].Value.ToString());
                phieuMuonDTO.ngayLapPhieu = DateTime.Now;
                phieuMuonBUS.createPhieuMuon(phieuMuonDTO);

                bool isCheck = false;
                foreach (var item in BUS_PhieuMuon.dictioLoanSlip)
                {
                    // Lưu chi tiết phiếu mượn
                    chiTietPhieuMuonDTO.idPhieuMuon = phieuMuonBUS.getIdPhieuMuon();
                    chiTietPhieuMuonDTO.maSach = item.Key.ToString();
                    chiTietPhieuMuonDTO.soLuong = int.Parse(item.Value.ToString());
                    chiTietPhieuMuonDTO.tinhTrang = "Phê duyệt";
                    if (chiTietPhieuMuonBUS.insertChiTietPhieuMuon(chiTietPhieuMuonDTO))
                    {
                        isCheck = true;
                    }
                }
                if (isCheck)
                {
                    Controller.isAlert(MdDocGia, "Thành công", "Phiếu mượn đã được gửi tới nhân viên", MessageDialogIcon.None);
                }    

                BUS_PhieuMuon.dictioLoanSlip.Clear();
                DgvPhieuMuon.Rows.Clear();
            }    
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            if (Controller.isEmpty(TbTuKhoa.Text))
            {
                string keyWord = TbTuKhoa.Text;

                DataTable data = sachBUS.dataSearchBooks(keyWord);
                Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
            }
            else
            {
                Controller.isAlert(MdDocGia, "Không hợp lệ", "Vui lòng nhập từ khóa!", MessageDialogIcon.Error);
            }
        }

        private void BtnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (Controller.isAllEmpty(TbMatKhauMoi.Text, TbNhapLaiMatKhau.Text))
            {
                if (TbMatKhauMoi.Text != TbNhapLaiMatKhau.Text)
                {
                    Controller.isAlert(MdDocGia, "Không hợp lệ", "Mật khẩu không khớp", MessageDialogIcon.Warning);
                }
                else
                {
                    quanLyNguoiDungDTO.matKhau = Controller.MD5Hash(TbMatKhauMoi.Text);
                    if (doiMatKhauBUS.updateMatKhau(quanLyNguoiDungDTO))
                    {
                        Controller.isAlert(MdDocGia, "Thành công", "Đổi mật khẩu thành công", MessageDialogIcon.None);
                        Controller.isResetTb(TbMatKhauMoi, TbNhapLaiMatKhau);
                    }
                }
            }
            else
            {
                Controller.isAlert(MdDocGia, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
            }
        }

        private void DgvLichSuPhieuMuon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        // ................................................

        // Sách thư viện
    }
}
