using DevExpress.Drawing.Internal.Fonts;
using DevExpress.Utils.DirectXPaint.Svg;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors.ViewInfo;
using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_BUS.BUS_NhanVien;
using Libary_Manager.Libary_BUS.BUS_QuanLy;
using Libary_Manager.Libary_BUS.Content;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_GUI
{
    public partial class Libary_NhanVien : Form
    {
        public Libary_NhanVien()
        {
            InitializeComponent();
        }


        private string codeTinh, codeHuyen;

        int _PAGE = 1;

        // Lưu tạm tên ảnh 
        private bool isClickPhoto = false;
        private string namePhotoBook;
        private string namePhotoPresent;

        // ................................................

        private BUS_Sach sachBUS;
        private BUS_PhieuMuon phieuMuonBUS;
        private BUS_ChiNhanh chiNhanhBUS;
        private BUS_DoiMatKhau doiMatKhauBUS;
        private BUS_ChamCong chamCongBUS;
        private BUS_QuanLyDocGia quanLyDocGiaBUS;
        private BUS_QuanLyNguoiDung quanLyNguoiDungBUS;
        private BUS_ChiTietPhieuMuon chiTietPhieuMuonBUS;

        // ................................................

        private DTO_Sach sachDTO;
        private DTO_PhieuMuon phieuMuonDTO;
        private DTO_QuanLyNguoiDung quanLyNguoiDungDTO;
        private DTO_ChamCong chamCongDTO;
        private DTO_ChiTietPhieuMuon chiTietPhieuMuonDTO;

        // ................................................

        private void Libary_NhanVien_Load(object sender, EventArgs e)
        {
            this.sachDTO = new DTO_Sach();
            this.phieuMuonDTO = new DTO_PhieuMuon();
            this.chamCongDTO = new DTO_ChamCong();
            this.quanLyNguoiDungDTO = new DTO_QuanLyNguoiDung();
            this.chiTietPhieuMuonBUS = new BUS_ChiTietPhieuMuon();

            this.sachBUS = new BUS_Sach();
            this.phieuMuonBUS = new BUS_PhieuMuon();
            this.chiNhanhBUS = new BUS_ChiNhanh();
            this.chamCongBUS = new BUS_ChamCong();
            this.quanLyDocGiaBUS = new BUS_QuanLyDocGia();
            this.quanLyNguoiDungBUS = new BUS_QuanLyNguoiDung();
            this.chiTietPhieuMuonDTO = new DTO_ChiTietPhieuMuon();

            // Kiểm tra chấm công hôm nay 
            checkChamCongToday();

            // Xóa các file
            Controller.proceedDeletePhotos();
        }

        // ................................................

        private void checkChamCongToday()
        {
            int hourRealTime = DateTime.Now.Hour;
            string caTruc = (hourRealTime < 11) ? "Sáng" : "Chiều";
            chamCongDTO.idNhanVien = DTO_DangNhap.id;
            chamCongDTO.tgBatDau = DateTime.Now;
            chamCongDTO.caTruc = caTruc;
            chamCongDTO.maChiNhanh = DTO_DangNhap.maChiNhanh;
            chamCongDTO.moTaSuco = "Không có";
            chamCongDTO.viPham = "Không";

            if (chamCongBUS.checkChamCongBuoiHomNay(chamCongDTO))
            {
                Controller.isAlert(MdNhanVien, "Thực hiện thành công", "Bạn đã chấm công, " + caTruc + " hôm nay", MessageDialogIcon.None);
            }
        }

        // Chọn quản lý sách sách
        private void TabSachThuVienAction()
        {
            TbTuaSach.Focus();
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");

            // Load toàn bộ Chi nhánh vào thêm sách
            CbbChiNhanh.DataSource = chiNhanhBUS.getChiNhanh();
            CbbChiNhanh.DisplayMember = "chiNhanh";
            CbbChiNhanh.ValueMember = "chiNhanh";
        }

        // ................................................

        // Chọn quản lý độc giả
        private void TabQuanLyDocGiaAction()
        {
            TbHoTen.Focus();

            // Giới tính
            CbGioiTinh.SelectedIndex = 0;

            DataTable dataTinh = quanLyNguoiDungBUS.getDsTinh();
            if (dataTinh != null)
            {
                CbTinh.DataSource = dataTinh;
                CbTinh.DisplayMember = "full_name";
                CbTinh.ValueMember = "code";
            }

            DgvDocGia.DataSource = quanLyDocGiaBUS.getDsDocGia();
        }

        // Chọn đổi mật khẩu 
        private void TabDoiMatKhauAction()
        {
            TbTaiKhoan.Focus();
            this.doiMatKhauBUS = new BUS_DoiMatKhau();

        }

        // Chọn chấm công 
        private void loadDsLichTrucTrongTuan(ArrayList data)
        {
            FlpLichTrucTuan.Controls.Clear();
            foreach (ArrayList array in data)
            {
                for (int i = 0; i < array.Count; i += 2)
                {
                    Label labelThu = new Label();
                    Label labelCaTruc = new Label();

                    labelThu.Text = array[i].ToString();
                    labelThu.Location = new Point(10, 10);
                    labelThu.BackColor = Color.Transparent;

                    labelCaTruc.Text = array[i + 1].ToString();
                    labelCaTruc.Location = new Point(10, 45);
                    labelCaTruc.BackColor = Color.Transparent;

                    Guna2GradientPanel panel = new Guna2GradientPanel();
                    panel.BorderRadius = 4;
                    panel.Size = new Size(100, 80);

                    labelThu.ForeColor = Color.White;
                    labelCaTruc.ForeColor = Color.White;

                    string soThu = new string(labelThu.Text.Where(char.IsDigit).ToArray());
                    int soThuInt;
                    int thuToday = (DTO_ChamCong.thuMay == 1) ? 8 : DTO_ChamCong.thuMay;
                    if (int.TryParse(soThu, out soThuInt))
                    {
                        soThuInt = int.Parse(soThu);
                    }
                    else
                    {
                        soThuInt = 8;
                    }

                    if (soThuInt < thuToday)
                    {
                        panel.FillColor = Color.Transparent;
                        panel.FillColor2 = Color.Transparent;
                        panel.BorderColor = Color.White;
                        panel.BorderThickness = 1;
                    }
                    else
                    {
                        panel.FillColor = Color.FromArgb(59, 130, 246);
                        panel.FillColor2 = Color.FromArgb(129, 140, 248);
                    }

                    panel.Controls.Add(labelThu);
                    panel.Controls.Add(labelCaTruc);

                    FlpLichTrucTuan.Controls.Add(panel);
                }
            }
        }

        private void TabChamCongAction()
        {
            ArrayList dataLichLam = new ArrayList();
            DataTable dataTuan = chamCongBUS.getDsLichTrucTrongTuan();

            foreach (DataRow row in dataTuan.Rows)
            {
                for (int i = 0; i < dataTuan.Columns.Count; i++)
                {
                    string[] separaThu = (row[i].ToString()).Split('|');

                    if (separaThu.Length > 1)
                    {
                        ArrayList array = new ArrayList();
                        for (int index = 0; index < separaThu.Length; ++index)
                        {
                            if (separaThu[index] != ""
                                && separaThu[index] != "-1"
                                && separaThu[index] == (DTO_DangNhap.id.ToString()))
                            {
                                string[] nameTable = dataTuan.Columns[i].ColumnName.Split('_');
                                if (nameTable.Length > 1)
                                {
                                    array.Add("Thứ " + nameTable[1]);
                                }
                                else
                                {
                                    array.Add("Chủ nhật");
                                }
                                if (index == 0) { array.Add("Sáng"); };
                                if (index == 1) { array.Add("Trưa"); };
                            }
                        }
                        dataLichLam.Add(array);
                    }
                }
            }

            loadDsLichTrucTrongTuan(dataLichLam);

            LbThu2.Text = "Thứ 2: " + Controller.GetThu2AndChuNhat()[0].ToString();
            LbChuNhat.Text = "Chủ nhật: " + Controller.GetThu2AndChuNhat()[1].ToString();

            // Load danh sách đã chấm công 
            DateTime dateTime = DateTime.Now;
            DataTable historyCcong = chamCongBUS.getDsChamCong(chamCongDTO, dateTime);
            DgvLsChamCong.DataSource = historyCcong;
        }

        // Chọn quản lý yêu cầu phiếu mượn 
        private void TabYeuCauMuonSachAction()
        {
            DataTable data = phieuMuonBUS.getYeuCauPhieuMuon();
            DgvYeuCauPhieuMuon.DataSource = data;
        }    

        // Load form tương thích
        private void TcLibaryQuanLy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabSachThuVien)
            {
                this.TabSachThuVienAction();
            }

            if (selectedTabPage == TabDoiMatKhau)
            {
                TbMatKhauMoi.Focus();
                this.TabDoiMatKhauAction();
            }    

            if (selectedTabPage == TabQuanLyDocGia)
            {
                this.TabQuanLyDocGiaAction();
            }

            if (selectedTabPage == TabChamCong)
            {
                this.TabChamCongAction();
            }

            if (selectedTabPage == TabYeuCauMuonSach)
            {
                this.TabYeuCauMuonSachAction();
            }    
        }

        private void DgvSachThuVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (sachBUS.deleteSach(sachDTO)) 
                {
                };
            }
        }

        private void DgvSachThuVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvSachThuVien.Rows[e.RowIndex];

                string maSach = row.Cells["maSach"].Value.ToString();
                string tuaSach = row.Cells["tuaSach"].Value.ToString();
                string tacGia = row.Cells["tacGia"].Value.ToString();
                string nhaXuatBan = row.Cells["nhaXuatBan"].Value.ToString();
                string loiGioiThieu = row.Cells["loiGioiThieu"].Value.ToString();
                string namXuatBan = row.Cells["namXuatBan"].Value.ToString();
                Image photo = (Image)row.Cells["photo"].Value;
                string soLuong = row.Cells["soLuong"].Value.ToString();
                string chiNhanh = row.Cells["chiNhanh"].Value.ToString();

                TbTuaSach.Text = tuaSach; TbTacGia.Text = tacGia; 
                TbNhaXuatBan.Text = nhaXuatBan; TbLoiGioiThieu.Text = loiGioiThieu; 
                TbNamXuatBan.Text = namXuatBan; TbSoLuong.Text = soLuong; PtXemAnhTruoc.Image = photo;

                sachDTO.maSach = maSach;

                // Lưu lại giá trị ảnh hiện tại 
                isClickPhoto = true;
            }
        }

        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = Controller.isOpenfile(OfdAnhSach);
            // người dùng đã chọn một tập tin hay không
            if (file != null)
            {
                PtXemAnhTruoc.Image = Image.FromFile(file.FileName);
                namePhotoBook = file.FileName;
                if (isClickPhoto)
                {
                    namePhotoPresent = file.FileName;
                }    
            }
        }

        private  bool isEmptys()
        {
            String[] data = { TbTuaSach.Text, TbTacGia.Text,
        TbNhaXuatBan.Text, TbNamXuatBan.Text, TbLoiGioiThieu.Text, TbSoLuong.Text };

            return Controller.isAllEmpty(data);
        }

        private void BtnThemSach_Click(object sender, EventArgs e)
        {
            if (isEmptys() && sachBUS.createMaSach() != null && namePhotoBook != null)
            {
                try
                {
                    sachDTO.maSach = sachBUS.createMaSach();
                    sachDTO.tuaSach = TbTuaSach.Text;
                    sachDTO.tacGia = TbTacGia.Text;
                    sachDTO.nhaXuatBan = TbNhaXuatBan.Text;
                    sachDTO.namXuatBan = TbNamXuatBan.Text;
                    sachDTO.maChiNhanh = chiNhanhBUS.getMaChiNhanh(CbbChiNhanh.Text);
                    sachDTO.loiGioiThieu = TbLoiGioiThieu.Text;
                    sachDTO.soLuong = int.Parse(TbSoLuong.Text);
                    sachDTO.photo = Controller.isSavedFile(namePhotoBook, "book");
                    sachDTO.ngayThem = DateTime.Now;

                    // Thêm sách
                    sachBUS.insertSach(sachDTO);

                    // Load sách mới thêm
                    DataTable data = sachBUS.getToanBoSach();
                    Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");

                    // Reset value 
                    CbbChiNhanh.SelectedIndex = 0;
                    Controller.isResetTb(TbTuaSach, TbTacGia, TbNhaXuatBan, TbLoiGioiThieu, TbNamXuatBan, TbSoLuong);
                }
                catch (Exception)
                {
                    Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập thông tin hợp lệ!", MessageDialogIcon.Error); return;
                }
            } 
            else
            {
                Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin!", MessageDialogIcon.Error);
            }
        }

        private void DgvSachThuVien_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvSachThuVien.Rows[e.RowIndex];

                sachDTO.maSach = row.Cells["maSach"].Value.ToString();
                sachDTO.tuaSach = row.Cells["tuaSach"].Value.ToString();
                sachDTO.tacGia = row.Cells["tacGia"].Value.ToString();
                sachDTO.nhaXuatBan = row.Cells["nhaXuatBan"].Value.ToString();
                sachDTO.namXuatBan = row.Cells["namXuatBan"].Value.ToString();
                sachDTO.maChiNhanh = chiNhanhBUS.getMaChiNhanh(CbbChiNhanh.Text);
                sachDTO.loiGioiThieu = row.Cells["loiGioiThieu"].Value.ToString();
                sachDTO.soLuong = int.Parse(row.Cells["soLuong"].Value.ToString());

                // Cập nhật sửa
                sachBUS.updateSach(sachDTO);
            }
        }

        private void BtnChinhSuaSach_Click(object sender, EventArgs e)
        {
            if (isEmptys())
            {
                try
                {
                    sachDTO.tuaSach = TbTuaSach.Text;
                    sachDTO.tacGia = TbTacGia.Text;
                    sachDTO.nhaXuatBan = TbNhaXuatBan.Text;
                    sachDTO.namXuatBan = TbNamXuatBan.Text;
                    sachDTO.maChiNhanh = chiNhanhBUS.getMaChiNhanh(CbbChiNhanh.Text);
                    sachDTO.loiGioiThieu = TbLoiGioiThieu.Text;
                    if (namePhotoPresent != null)
                    {
                        string pathImage = sachBUS.prepareDeletePhoto(sachDTO) + ";";
                        Controller.DeletedPhotos += pathImage;
                        sachDTO.photo = Controller.isSavedFile(namePhotoPresent, "book");
                    }
                    sachDTO.soLuong = int.Parse(TbSoLuong.Text);

                    // Cập nhật sửa
                    sachBUS.updateSach(sachDTO);

                    // Load sách mới chỉnh
                    DataTable data = sachBUS.dataFullPagination(_PAGE);
                    Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
                }
                catch (Exception)
                {
                    Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập thông tin hợp lệ!", MessageDialogIcon.Error); return;
                }
            }
            else
            {
                Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng chọn 1 hàng thông tin!", MessageDialogIcon.Error);
            }
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
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
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
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnTrangDau_Click(object sender, EventArgs e)
        {
            _PAGE = 1;
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnTrangCuoi_Click(object sender, EventArgs e)
        {
            _PAGE = Convert.ToInt32(Math.Ceiling((double)BUS_Sach._TOTAL_BOOK / Controller._MAX_PAGE));
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private void BtnHuyChon_Click(object sender, EventArgs e)
        {
            Controller.isResetTb(TbTuaSach, TbTacGia, TbNhaXuatBan, TbNamXuatBan, TbLoiGioiThieu, TbSoLuong);
            CbbChiNhanh.SelectedIndex = 0;
            PtXemAnhTruoc.Image = null;
        }

        private void BtnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (Controller.isAllEmpty(TbMatKhauMoi.Text, TbNhapLaiMatKhau.Text))
            {
                if (TbMatKhauMoi.Text != TbNhapLaiMatKhau.Text)
                {
                    Controller.isAlert(MdNhanVien, "Không hợp lệ", "Mật khẩu không khớp", MessageDialogIcon.Warning);
                }
                else
                {
                    quanLyNguoiDungDTO.matKhau = Controller.MD5Hash(TbMatKhauMoi.Text);
                    if (doiMatKhauBUS.updateMatKhau(quanLyNguoiDungDTO))
                    {
                        Controller.isAlert(MdNhanVien, "Thành công", "Đổi mật khẩu thành công", MessageDialogIcon.None);
                        Controller.isResetTb(TbMatKhauMoi, TbNhapLaiMatKhau);
                    }    
                }
            }
            else
            {
                Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
            }
        }

        private void DgvSachThuVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in DgvSachThuVien.Rows)
            {
                if (!row.IsNewRow && row.Cells["chiNhanh"].Value != null)
                {
                    if (row.Cells["chiNhanh"].Value.ToString() == "")
                    {
                        row.Cells["chiNhanh"].Style.SelectionBackColor = Color.Crimson;
                        row.Cells["chiNhanh"].Style.BackColor = Color.Crimson;
                    }
                }
            }
        }

        // Ẩn phân trang 
        private void identifyPagination(string type = "off")
        {
            bool identify = (type == "on") ? true : false;
            BtnQuayLai.Visible = identify; BtnTiepTuc.Visible = identify;
            BtnTrangDau.Visible = identify; BtnTrangCuoi.Visible = identify;
        }    

        // Tìm kiếm sách
        private void TbTimKiem_Click(object sender, EventArgs e)
        {
            if (Controller.isEmpty(TbTuKhoa.Text))
            {
                // Load toàn bộ danh sách Sách
                DataTable data = sachBUS.dataSearchBooks(TbTuKhoa.Text);
                if (data != null)
                {
                    identifyPagination();
                    BtnResetSearch.Visible = true;
                    Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
                }    
            }
            else
            {
                Controller.isAlert(MdNhanVien, "Sự cố xảy ra", "Nhập vào ít nhất 1 tựa sách", MessageDialogIcon.Error);
            }
        }

        private void BtnResetSearch_Click(object sender, EventArgs e)
        {
            identifyPagination("on");
            BtnResetSearch.Visible = false;
            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataFullPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        private bool checkTaiKhoanTonTai()
        {
            if (Controller.isEmpty(TbTaiKhoan.Text))
            {
                if (quanLyNguoiDungBUS.checkTypeValueExists("taiKhoan", TbTaiKhoan.Text))
                {
                    Controller.isAlert(MdNhanVien, "Xảy ra lỗi", "Tài khoản đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                    TbTaiKhoan.Text = ""; return false;
                }
                return true;
            }
            return false;
        }

        private bool checkEmailTonTai()
        {
            if (Controller.isEmpty(TbEmail.Text))
            {
                if (!TbEmail.Text.Contains("@gmail.com") || TbEmail.Text.Length < "@gmail.com".Length)
                {
                    Controller.isAlert(MdNhanVien, "Không hợp lệ", "Email phải chứa '@gmail.com' và có từ 11 kí tự", MessageDialogIcon.Error);
                    TbEmail.Text = ""; return false;
                }
                else
                {
                    if (quanLyNguoiDungBUS.checkTypeValueExists("email", TbEmail.Text))
                    {
                        Controller.isAlert(MdNhanVien, "Xảy ra lỗi", "Email đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                        TbEmail.Text = ""; return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool checkMssvTonTai()
        {
            if (Controller.isEmpty(TbMaSinhVien.Text))
            {
                if (quanLyNguoiDungBUS.checkTypeValueExists("mssv", TbTaiKhoan.Text))
                {
                    Controller.isAlert(MdNhanVien, "Xảy ra lỗi", "Mã số sinh viên đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                    TbMaSinhVien.Text = ""; return false;
                }
                else if (!Controller.isLength(TbMaSinhVien.Text, 13))
                {
                    Controller.isAlert(MdNhanVien, "Xảy ra lỗi", "Mã số sinh viên phải đủ 11 kí tự", MessageDialogIcon.Error);
                    TbMaSinhVien.Text = ""; return false;
                }    
                return true;
            }
            return false;
        }

        private void TbTaiKhoan_Leave(object sender, EventArgs e)
        {
            this.checkTaiKhoanTonTai();
        }

        private void TbEmail_Leave(object sender, EventArgs e)
        {
            this.checkEmailTonTai();
        }

        private void TbMaSinhVien_Leave(object sender, EventArgs e)
        {
            this.checkMssvTonTai();
        }

        private bool isEmptysDg()
        {
            return Controller.isAllEmpty(TbTaiKhoan.Text, TbHoTen.Text, TbEmail.Text, CbGioiTinh.Text, CbGioiTinh.Text,
                CbHuyen.Text, CbXa.Text, DtpNgaySinh.Text);
        }

        private void resetValueControl()
        {
            Controller.isResetTb(TbHoTen, TbTaiKhoan, TbEmail, TbMaSinhVien);
            CbTinh.SelectedIndex = 0;
            CbGioiTinh.SelectedIndex = 0;
            DtpNgaySinh.Value = DateTime.Now;
        }

        private void BtnThemDocGia_Click(object sender, EventArgs e)
        {
            string[] toEmail = { TbEmail.Text };
            string randomMatKhau = quanLyNguoiDungBUS.GenerateRandomPassword(13);
            string hashMatKhau = Controller.MD5Hash(randomMatKhau);

            if (this.isEmptysDg() && Controller.isEmpty(hashMatKhau) && 
                checkEmailTonTai() && checkTaiKhoanTonTai() && checkMssvTonTai()) 
            {
                Controller.isSendToEmails(toEmail, "📢eBook Gửi thông tin tài khoản độc giả của bạn", ContentEmail.ctCreatenDocGia(TbHoTen.Text, TbTaiKhoan.Text, randomMatKhau));
                Controller.isAlert(MdNhanVien, "Cấp thành công tài khoản", "Thông tin tài khoản đã gưi tới độc giả", MessageDialogIcon.None);
                string diaChi = CbTinh.Text + "|" + CbHuyen.Text + "|" + CbXa.Text;
                quanLyNguoiDungDTO.taiKhoan = TbTaiKhoan.Text;
                quanLyNguoiDungDTO.matKhau = hashMatKhau;
                quanLyNguoiDungDTO.hoTen = TbHoTen.Text;
                quanLyNguoiDungDTO.quyen = 2;
                quanLyNguoiDungDTO.email = TbEmail.Text;
                quanLyNguoiDungDTO.mssv = TbMaSinhVien.Text;
                quanLyNguoiDungDTO.gioiTinh = CbGioiTinh.Text;
                quanLyNguoiDungDTO.diaChi = diaChi;
                quanLyNguoiDungDTO.ngaySinh = DtpNgaySinh.Value;

                // Thêm độc giả
                quanLyDocGiaBUS.insertDocGia(quanLyNguoiDungDTO);
                this.resetValueControl();
                DgvDocGia.DataSource = quanLyDocGiaBUS.getDsDocGia();
            }
            else
            {
                Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
                return;
            }
        }

        private void CbTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeTinh = CbTinh.SelectedValue.ToString();

            DataTable dataHuyen = quanLyNguoiDungBUS.getDsHuyen(codeTinh);
            if (dataHuyen != null)
            {
                CbHuyen.DataSource = dataHuyen;
                CbHuyen.DisplayMember = "full_name";
                CbHuyen.ValueMember = "code";
            }
        }

        private void CbHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeHuyen = CbHuyen.SelectedValue.ToString();

            DataTable dataXa = quanLyNguoiDungBUS.getDsXa(codeHuyen);
            if (dataXa != null)
            {
                CbXa.DataSource = dataXa;
                CbXa.DisplayMember = "full_name";
                CbXa.ValueMember = "full_name";
            }
        }

        private void chapNhanYeuCauMuon(int id)
        {
            // edit phiếu mượn
            phieuMuonDTO.idNhanVien = DTO_DangNhap.id;
            phieuMuonDTO.idChiNhanh = DTO_DangNhap.maChiNhanh;

            // edit chi tiết chi nhánh
            chiTietPhieuMuonDTO.idPhieuMuon = id;
            chiTietPhieuMuonDTO.tinhTrang = "Đang mượn";
            chiTietPhieuMuonDTO.ngayMuon = DateTime.Now;
            chiTietPhieuMuonDTO.ngayTra = chiTietPhieuMuonDTO.ngayMuon.AddDays(30);

            phieuMuonBUS.chapNhanYeuCauMuon(phieuMuonDTO);

            DataTable data = phieuMuonBUS.getDsYeuCauMuonSach();
            DgvYeuCauPhieuMuon.DataSource = data;
        }    

        private void huyBoYeuCauMuon(int id)
        {
            // edit phiếu mượn
            phieuMuonDTO.idNhanVien = DTO_DangNhap.id;
            phieuMuonDTO.idChiNhanh = DTO_DangNhap.maChiNhanh;

            // edit chi tiết mượn sách 
            chiTietPhieuMuonDTO.idPhieuMuon = id;
            chiTietPhieuMuonDTO.tinhTrang = "Từ chối";

            phieuMuonBUS.huyBoYeuCauMuon(phieuMuonDTO);
            DataTable data = phieuMuonBUS.getDsYeuCauMuonSach();
            DgvYeuCauPhieuMuon.DataSource = data;
        }

        private void TbNhapMaSinhVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Controller.isEmpty(TbNhapMaSinhVien.Text))
                {
                    phieuMuonDTO.id = phieuMuonDTO.id;
                }    
                else
                {
                    Controller.isAlert(MdNhanVien, "Không hợp lệ", "Vui lòng nhập vào mã sinh viên", MessageDialogIcon.Error);
                }
            }    
        }

        // Yêu cầu mượn sách
        private void DgvYeuCauPhieuMuon_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = DgvYeuCauPhieuMuon.Rows[e.RowIndex].Cells["ycId"];

                // Chấp nhận 
                if (e.ColumnIndex == 0)
                {
                    if (cell != null && cell.Value != null)
                    {
                        int id = (int)cell.Value;
                        this.chapNhanYeuCauMuon(id);
                    }
                }

                // Xóa bỏ 
                if (e.ColumnIndex == 1)
                {
                    if (cell != null && cell.Value != null)
                    {
                        int id = (int)cell.Value;
                        this.huyBoYeuCauMuon(id);
                    }
                }
            }
        }
    }
}

