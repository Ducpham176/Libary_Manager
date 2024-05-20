using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_BUS.BUS_QuanLy;
using Libary_Manager.Libary_BUS.Content;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Libary_Manager.Libary_GUI
{
    public partial class Libary_QuanLy : Form
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

        public Libary_QuanLy()
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.activedDoubleBuffered(DgvChiNhanh);
            this.activedDoubleBuffered(DgvNhanVien);
        }


        private string codeTinh, codeHuyen;

        // ................................................

        private BUS_DoiMatKhau doiMatKhauBUS;
        private BUS_ChiNhanh chiNhanhBUS;
        private BUS_QuanLyNguoiDung quanLyNguoiDungBUS;
        private BUS_QuanLyNhanVien quanLyNhanVienBUS;
        private BUS_PhanCongNhanVien phanCongNhanVienBUS;

        // ................................................

        private DTO_ChiNhanh chiNhanhDTO;
        private DTO_QuanLyNguoiDung quanLyNguoiDungDTO;
        private DTO_PhanCongNhanVien phanCongNhanVienDTO;

        // ................................................

        private void Libary_QuanLy_Load(object sender, EventArgs e)
        {
            this.chiNhanhDTO = new DTO_ChiNhanh();
            this.quanLyNguoiDungDTO = new DTO_QuanLyNguoiDung();
            this.phanCongNhanVienDTO = new DTO_PhanCongNhanVien();

            this.chiNhanhBUS = new BUS_ChiNhanh();
            this.quanLyNguoiDungBUS = new BUS_QuanLyNguoiDung();
            this.quanLyNhanVienBUS = new BUS_QuanLyNhanVien();
            this.quanLyNhanVienBUS = new BUS_QuanLyNhanVien();
            this.phanCongNhanVienBUS = new BUS_PhanCongNhanVien();

            this.TabThongTinAction();
        }

        // ................................................

        // Thông tin 
        private void TabThongTinAction()
        {
            LbQuyen.Text = Controller.isRevertTypeQuyen();
            LbHoTen.Text = DTO_DangNhap.hoTen;
            LbGioiTinh.Text = DTO_DangNhap.gioiTinh;
            LbNgaySinh.Text = DTO_DangNhap.ngaySinh.ToString();
            LbNgayThamGia.Text = DTO_DangNhap.ngayTao.ToString();
            LbDiaChi.Text = DTO_DangNhap.diaChi;
        }

        // Chọn đổi mật khẩu
        private void TabDoiMatKhauAction()
        {
            TbTaiKhoan.Focus();
            this.doiMatKhauBUS = new BUS_DoiMatKhau();
            this.quanLyNguoiDungDTO = new DTO_QuanLyNguoiDung();
        }

        // Chọn quản lý chi nhánh 
        private void TabChiNhanhAction()
        {
            // Chí nhánh 
            DataTable data = chiNhanhBUS.getChiNhanh();
            if (data != null)
            {
                DgvChiNhanh.DataSource = data;
            }    
        }

        // Chọn quản lý nhân viên 
        private void TabQuanLyNhanVienAction()
        {
            TbHoTen.Focus();

            // Giới tính
            CbbGioiTinh.SelectedIndex = 0;

            // Trạng thái làm việc 
            CbbTrangThai.SelectedIndex = 0;

            DataTable dataTinh = quanLyNguoiDungBUS.getDsTinh();
            if (dataTinh != null)
            {
                CbbTinh.DataSource = dataTinh;
                CbbTinh.DisplayMember = "full_name";
                CbbTinh.ValueMember = "code";
            }
            
            DataTable dataCn = chiNhanhBUS.getChiNhanh();
            if (dataCn != null)
            {
                CbbChiNhanhNv.DataSource = dataCn;
                CbbChiNhanhNv.DisplayMember = "chiNhanh";
                CbbChiNhanhNv.ValueMember = "id";
            }

            DgvNhanVien.DataSource = quanLyNhanVienBUS.getDsNhanVien();
        }

        private void setValueCombobox(Guna2ComboBox Cbbox)
        {
            DataTable dtNhanVien = quanLyNhanVienBUS.getDsNhanVienTheoChiNhanh(quanLyNguoiDungDTO);

            if (dtNhanVien != null)
            {
                DataRow drNoData = dtNhanVien.NewRow();
                drNoData["id"] = -1;
                drNoData["hoTen"] = "- Chọn";
                dtNhanVien.Rows.InsertAt(drNoData, 0);

                Cbbox.DataSource = dtNhanVien;
                Cbbox.DisplayMember = "hoTen";
                Cbbox.ValueMember = "id";
            }    
        }

        private void setValueSelected(DataTable data, ComboBox cbbST, ComboBox cbbTC, string name)
        {
            string[] ArrThu = data.Rows[0][name].ToString().Split('|');
            
            if (data.Rows[0][name].ToString() != "")
            {
                if (ArrThu[0].ToString() != "")
                {
                    cbbST.SelectedValue = int.Parse(ArrThu[0]);
                }
                if (ArrThu[1].ToString() != "")
                {
                    cbbTC.SelectedValue = int.Parse(ArrThu[1]);
                }
            }
            else
            {
                CbSTThu2.SelectedIndex = 0; CbTCThu2.SelectedIndex = 0;
            }
        }

        private void loadPhanCongExists(DataTable data)
        {
            if (data.Rows.Count > 0)
            {
                this.setValueSelected(data, CbSTThu2, CbTCThu2, "idThu2");
                this.setValueSelected(data, CbSTThu3, CbTCThu3, "idThu3");
                this.setValueSelected(data, CbSTThu4, CbTCThu4, "idThu4");
                this.setValueSelected(data, CbSTThu5, CbTCThu5, "idThu5");
                this.setValueSelected(data, CbSTThu6, CbTCThu6, "idThu6");
                this.setValueSelected(data, CbSTThu7, CbTCThu7, "idThu7");
                this.setValueSelected(data, CbSTThuCN, CbTCThuCN, "idChuNhat");
            }    
        }

        private void setValueNhanVienOnCbb()
        {
            setValueCombobox(CbSTThu2);
            setValueCombobox(CbTCThu2);

            setValueCombobox(CbSTThu3);
            setValueCombobox(CbTCThu3);

            setValueCombobox(CbSTThu4);
            setValueCombobox(CbTCThu4);

            setValueCombobox(CbSTThu5);
            setValueCombobox(CbTCThu5);

            setValueCombobox(CbSTThu6);
            setValueCombobox(CbTCThu6);

            setValueCombobox(CbSTThu7);
            setValueCombobox(CbTCThu7);

            setValueCombobox(CbSTThuCN);
            setValueCombobox(CbTCThuCN);
        }

        private void TabPhanCongNVAction()
        {

            DataTable dtChiNhanh = chiNhanhBUS.getChiNhanh();
            if (dtChiNhanh != null)
            {
                CbChiNhanh.DataSource = dtChiNhanh;
                CbChiNhanh.DisplayMember = "chiNhanh";
                CbChiNhanh.ValueMember = "id";
            }

            quanLyNguoiDungDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());

            this.setValueNhanVienOnCbb();

            LbValueThu2.Text = "Thứ 2: " + Controller.GetThu2AndChuNhat()[0].ToString();
            LbValueCn.Text = "Chủ nhật: " + Controller.GetThu2AndChuNhat()[1].ToString();

            phanCongNhanVienDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());
            // Load dữ liệu nếu đã tồn tại 
            DataTable dataPhanCong = phanCongNhanVienBUS.dataPhieuPhanCongNhanVien(phanCongNhanVienDTO);
            this.loadPhanCongExists(dataPhanCong);
        }

        // Đăng xuất 
        private void TabDangXuatAction()
        {
            bool userConfirmed = Controller.isQuestion(MdQuanLy, "Xác nhận hành động", "Bạn có chắc chắn muốn đăng xuất?");
            if (userConfirmed)
            {
                this.Close();
            }
            else
            {
                TcLibaryQuanLy.SelectedTab = TabThongTin;
            }
        }

        // ................................................

        // Load form tương thích
        private void TcLibaryQuanLy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabThongTin)
            {
                this.TabThongTinAction();
            }    

            if (selectedTabPage == TabDoiMatKhau)
            {
                this.TabDoiMatKhauAction();
            }    

            if (selectedTabPage == TabChiNhanh)
            {
                this.TabChiNhanhAction();
            }

            if (selectedTabPage == TabQlNhanVien)
            {
                this.TabQuanLyNhanVienAction();
            }    

            if (selectedTabPage == TabPhanCongNV)
            {

                this.TabPhanCongNVAction();
            }

            if (selectedTabPage == TabDangXuat)
            {

                this.TabDangXuatAction();
            }
        }

        // ................................................

        private void BtnChiNhanh_Click(object sender, EventArgs e)
        {
            if (Controller.isEmpty(TbChiNhanh.Text) && Controller.isEmpty(TbDiaChi.Text))
            {
                chiNhanhDTO.chiNhanh = TbChiNhanh.Text;
                chiNhanhDTO.diaChi = TbDiaChi.Text;
                chiNhanhDTO.ngayThem = DateTime.Now;

                // Thêm chi nhánh
                if (chiNhanhBUS.insertChiNhanh(chiNhanhDTO))
                {
                    DgvChiNhanh.DataSource = chiNhanhBUS.getChiNhanh();
                    Controller.isResetTb(TbChiNhanh, TbDiaChi);
                }
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin!", MessageDialogIcon.Error);
            }
        }

        // Chi nhánh

        // Set giá trị hàng click
        private void DgvChiNhanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvChiNhanh.Rows[e.RowIndex];
                object value = row.Cells[0].Value;

                if (value != null)
                {
                    chiNhanhDTO.id = int.Parse(value.ToString());
                }
            }
        }

        private void DgvChiNhanh_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvChiNhanh.Rows[e.RowIndex];
                chiNhanhDTO.chiNhanh = row.Cells["chiNhanh"].Value.ToString();
                chiNhanhDTO.diaChi = row.Cells["diaChi"].Value.ToString();

                if (Controller.isAllEmpty(chiNhanhDTO.chiNhanh, chiNhanhDTO.diaChi))
                {
                    chiNhanhBUS.updateChiNhanh(chiNhanhDTO);
                }
            }
        }

        // Nhấn delete xóa
        private void DgvChiNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (chiNhanhBUS.deleteChiNhanh(chiNhanhDTO)) { };
            }
        }

        private void CbTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeTinh = CbbTinh.SelectedValue.ToString();

            DataTable dataHuyen = quanLyNguoiDungBUS.getDsHuyen(codeTinh);
            if (dataHuyen != null)
            {
                CbbHuyen.DataSource = dataHuyen;
                CbbHuyen.DisplayMember = "full_name";
                CbbHuyen.ValueMember = "code";
            }
        }

        private void CbHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeHuyen = CbbHuyen.SelectedValue.ToString();

            DataTable dataXa = quanLyNguoiDungBUS.getDsXa(codeHuyen);
            if (dataXa != null)
            {
                CbbXa.DataSource = dataXa;
                CbbXa.DisplayMember = "full_name";
                CbbXa.ValueMember = "full_name";
            }
        }


        private bool checkTaiKhoanTonTai()
        {
            if (Controller.isEmpty(TbTaiKhoan.Text))
            {
                if (quanLyNguoiDungBUS.checkTypeValueExists("taiKhoan", TbTaiKhoan.Text))
                {
                    Controller.isAlert(MdQuanLy, "Xảy ra lỗi", "Tài khoản đã tồn tại trên hệ thống", MessageDialogIcon.Error);
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
                    Controller.isAlert(MdQuanLy, "Không hợp lệ", "Email phải chứa '@gmail.com' và có từ 11 kí tự", MessageDialogIcon.Error);
                    TbEmail.Text = ""; return false;
                }
                else
                {
                    if (quanLyNguoiDungBUS.checkTypeValueExists("email", TbEmail.Text))
                    {
                        Controller.isAlert(MdQuanLy, "Xảy ra lỗi", "Email đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                        TbEmail.Text = ""; return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool isEmptysNV()
        {
            return Controller.isAllEmpty(TbTaiKhoan.Text, TbHoTen.Text, CbbTrangThai.Text, TbEmail.Text, CbbGioiTinh.Text,
                CbbHuyen.Text, CbbXa.Text, DtpNgaySinh.Text);
        }

        private void resetValueControl()
        {
            Controller.isResetTb(TbHoTen, TbTaiKhoan, TbEmail);
            CbbTinh.SelectedIndex = 0;
            CbbGioiTinh.SelectedIndex = 0;
            DtpNgaySinh.Value = DateTime.Now;
        }    

        private void BtnThemNhanVien_Click(object sender, EventArgs e)
        { 
            string[] toEmail = { TbEmail.Text };
            string randomMatKhau = quanLyNguoiDungBUS.GenerateRandomPassword(13);
            string hashMatKhau = Controller.MD5Hash(randomMatKhau);

            if (this.isEmptysNV() && Controller.isEmpty(hashMatKhau) && checkEmailTonTai() && checkTaiKhoanTonTai())
            {
                Controller.isSendToEmails(toEmail, "📢eBook Gửi thông tin tài khoản nhân viên của bạn", ContentEmail.ctCreateNhanVien(TbHoTen.Text, TbTaiKhoan.Text, randomMatKhau));
                Controller.isAlert(MdQuanLy, "Cấp thành công tài khoản", "Thông tin tài khoản đã gưi tới nhân viên", MessageDialogIcon.None);
                string diaChi = CbbTinh.Text + "|" + CbbHuyen.Text + "|" + CbbXa.Text;
                int trangThai = (CbbTrangThai.SelectedIndex == 0) ? 1 : -1;
                quanLyNguoiDungDTO.taiKhoan = TbTaiKhoan.Text;
                quanLyNguoiDungDTO.matKhau = hashMatKhau;
                quanLyNguoiDungDTO.hoTen = TbHoTen.Text;
                quanLyNguoiDungDTO.quyen = 1;
                quanLyNguoiDungDTO.maChiNhanh = int.Parse(CbbChiNhanhNv.SelectedValue.ToString());
                quanLyNguoiDungDTO.trangThai = trangThai;
                quanLyNguoiDungDTO.email = TbEmail.Text;
                quanLyNguoiDungDTO.gioiTinh = CbbGioiTinh.Text;
                quanLyNguoiDungDTO.diaChi = diaChi;
                quanLyNguoiDungDTO.ngaySinh = DtpNgaySinh.Value;

                // Thêm nhân viên
                quanLyNhanVienBUS.insertNhanVien(quanLyNguoiDungDTO);
                this.resetValueControl();
                DgvNhanVien.DataSource = quanLyNhanVienBUS.getDsNhanVien();
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
                return;
            }
        }

        private void BtnChinhSua_Click(object sender, EventArgs e)
        {
            if (Controller.isEmpty(TbHoTen.Text))
            {
                string diaChi = CbbTinh.Text + "|" + CbbHuyen.Text + "|" + CbbXa.Text;
                int trangThai = (CbbTrangThai.SelectedIndex == 0) ? 1 : -1;
                quanLyNguoiDungDTO.hoTen = TbHoTen.Text;
                quanLyNguoiDungDTO.quyen = 1;
                quanLyNguoiDungDTO.maChiNhanh = int.Parse(CbbChiNhanhNv.SelectedValue.ToString());
                quanLyNguoiDungDTO.trangThai = trangThai;
                quanLyNguoiDungDTO.gioiTinh = CbbGioiTinh.Text;
                quanLyNguoiDungDTO.diaChi = diaChi;
                quanLyNguoiDungDTO.ngaySinh = DtpNgaySinh.Value;

                // Chỉnh sửa nhân viên
                quanLyNhanVienBUS.updateNhanVien(quanLyNguoiDungDTO);
                DgvNhanVien.DataSource = quanLyNhanVienBUS.getDsNhanVien();
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
            }
        }

        private void TbTaiKhoan_Leave(object sender, EventArgs e)
        {
            checkTaiKhoanTonTai();
        }

        private void TbEmail_Leave(object sender, EventArgs e)
        {
            checkEmailTonTai();
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvNhanVien.Rows[e.RowIndex];

                string hoTen = row.Cells["hoTen"].Value.ToString();
                string taiKhoan = row.Cells["taiKhoan"].Value.ToString();
                string diaChi = row.Cells["nvDiaChi"].Value.ToString();
                string ngaySinh = row.Cells["ngaySinh"].Value.ToString();
                string gioiTinh = row.Cells["gioiTinh"].Value.ToString();
                string chiNhanh = row.Cells["nvChiNhanh"].Value.ToString();
                string trangThai = row.Cells["trangThai"].Value.ToString();

                TbHoTen.Text = hoTen; 
                string[] arrayDiaChi = diaChi.Split('|');
                CbbTinh.Text = arrayDiaChi[0];
                CbbHuyen.Text = arrayDiaChi[1];
                CbbXa.Text = arrayDiaChi[2];

                quanLyNguoiDungDTO.taiKhoan = taiKhoan;

                DateTime ngaySinhRevert;
                if (DateTime.TryParse(ngaySinh, out ngaySinhRevert))
                {
                    DtpNgaySinh.Value = ngaySinhRevert;
                }
                CbbGioiTinh.Text = gioiTinh;
                CbbChiNhanhNv.Text = chiNhanh;
                CbbTrangThai.Text = trangThai;
            }
        }

        private void BtnHuyChon_Click(object sender, EventArgs e)
        {
            this.resetValueControl();
        }

        private void DgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in DgvNhanVien.Rows)
            {
                if (!row.IsNewRow && row.Cells["trangThai"].Value != null)
                {
                    if (row.Cells["trangThai"].Value.ToString() == "Đang làm việc")
                    {
                        row.Cells["trangThai"].Style.SelectionForeColor = Color.MediumSpringGreen;
                        row.Cells["trangThai"].Style.ForeColor = Color.MediumSpringGreen;
                    }
                    else
                    {
                        row.Cells["trangThai"].Style.SelectionForeColor = Color.Crimson;
                        row.Cells["trangThai"].Style.ForeColor = Color.Crimson;
                    }
                }
            }
        }

        private string handlePhanCong(ComboBox St, ComboBox Tc)
        {
            if (Controller.isEmpty(St.Text) || Controller.isEmpty(Tc.Text))
            {
                string idNhanVienThu = St.SelectedValue + "|" + Tc.SelectedValue;
                return idNhanVienThu;
            }
            return "";
        }    

        private void BtnLuuPhanCong_Click(object sender, EventArgs e)
        {
            phanCongNhanVienDTO.idThu2 = this.handlePhanCong(CbSTThu2, CbTCThu2);
            phanCongNhanVienDTO.idThu3 = this.handlePhanCong(CbSTThu3, CbTCThu3);
            phanCongNhanVienDTO.idThu4 = this.handlePhanCong(CbSTThu4, CbTCThu4);
            phanCongNhanVienDTO.idThu5 = this.handlePhanCong(CbSTThu5, CbTCThu5);
            phanCongNhanVienDTO.idThu6 = this.handlePhanCong(CbSTThu6, CbTCThu6);
            phanCongNhanVienDTO.idThu7 = this.handlePhanCong(CbSTThu7, CbTCThu7);
            phanCongNhanVienDTO.idChuNhat = this.handlePhanCong(CbSTThuCN, CbTCThuCN);
            phanCongNhanVienDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());

            phanCongNhanVienBUS.savedPhanCongNhanVien(phanCongNhanVienDTO);
        }


        private DataTable loadThongTinTheoThu(DTO_PhanCongNhanVien phanCongNhanVienDTO, string thu)
        {
            phanCongNhanVienDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());
            return phanCongNhanVienBUS.getThongTinTheoThu(phanCongNhanVienDTO, thu);
        }

        private void GrThu2_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu2");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            } 
            else
            {
                DgvThongTinPhanCong.Rows.Clear();
            }
        }

        private void GrThu3_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu3");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void GrThu4_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu4");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void GrThu5_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu5");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void GrThu6_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu6");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void GrThu7_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idThu7");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void GrChuNhat_Click(object sender, EventArgs e)
        {
            DataTable data = this.loadThongTinTheoThu(phanCongNhanVienDTO, "idChuNhat");
            if (data != null)
            {
                DgvThongTinPhanCong.DataSource = data;
            }
        }

        private void BtnDatLaiPhanCong_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            this.loadPhanCongExists(data);
        }

        private void CbChiNhanh_SelectionChangeCommitted(object sender, EventArgs e)
        {
            quanLyNguoiDungDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());
            this.setValueNhanVienOnCbb();
            phanCongNhanVienDTO.maChiNhanh = int.Parse(CbChiNhanh.SelectedValue.ToString());

            // Load dữ liệu nếu đã tồn tại 
            DataTable dataPhanCong = phanCongNhanVienBUS.dataPhieuPhanCongNhanVien(phanCongNhanVienDTO);
            this.loadPhanCongExists(dataPhanCong);
        }

        private void BtnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (Controller.isAllEmpty(TbMatKhauMoi.Text, TbNhapLaiMatKhau.Text))
            {
                if (TbMatKhauMoi.Text != TbNhapLaiMatKhau.Text)
                {
                    Controller.isAlert(MdQuanLy, "Không hợp lệ", "Mật khẩu không khớp", MessageDialogIcon.Warning);
                }
                else
                {
                    quanLyNguoiDungDTO.matKhau = Controller.MD5Hash(TbMatKhauMoi.Text);
                    if (doiMatKhauBUS.updateMatKhau(quanLyNguoiDungDTO))
                    {
                        Controller.isAlert(MdQuanLy, "Thành công", "Đổi mật khẩu thành công", MessageDialogIcon.None);
                        Controller.isResetTb(TbMatKhauMoi, TbNhapLaiMatKhau);
                    }
                }
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
            }
        }

        // ................................................
    }
}
