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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_GUI
{
    public partial class Libary_QuanLy : Form
    {
        private string codeTinh, codeHuyen; 

        // ................................................

        private BUS_Sach sachBUS;
        private BUS_ChiNhanh chiNhanhBUS;
        private BUS_QuanLyNguoiDung quanLyNguoiDungBUS;
        private BUS_QuanLyNhanVien quanLyNhanVienBUS;

        // ................................................

        private DTO_Sach sachDTO;
        private DTO_ChiNhanh chiNhanhDTO;
        private DTO_QuanLyNguoiDung quanLyNguoiDungDTO;

        // ................................................

        public Libary_QuanLy()
        {
            InitializeComponent();
        }

        // ................................................
        // Chọn đổi mật khẩu
        private void TabDoiMatKhauAction()
        {
            TbTaiKhoan.Focus();
        }

        // Chọn quản lý chi nhánh 
        private void TabChiNhanhAction()
        {
            // Chí nhánh 
            this.chiNhanhBUS = new BUS_ChiNhanh();
            this.chiNhanhDTO = new DTO_ChiNhanh();
            DgvChiNhanh.DataSource = chiNhanhBUS.getToanBoSach();

        }

        // Chọn quản lý nhân viên 
        private void TabQuanLyNhanVienAction()
        {
            TbHoTen.Focus();

            // Giới tính
            CbGioiTinh.SelectedIndex = 0;

            // Trạng thái làm việc 
            CbTrangThai.SelectedIndex = 0;

            this.quanLyNguoiDungBUS = new BUS_QuanLyNguoiDung();
            this.quanLyNguoiDungDTO = new DTO_QuanLyNguoiDung();
            this.quanLyNhanVienBUS = new BUS_QuanLyNhanVien();

            DataTable dataTinh = quanLyNguoiDungBUS.getDsTinh();
            if (dataTinh != null)
            {
                CbTinh.DataSource = dataTinh;
                CbTinh.DisplayMember = "full_name";
                CbTinh.ValueMember = "code";
            }

            DgvNhanVien.DataSource = quanLyNhanVienBUS.getDsNhanVien();
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
                    DgvChiNhanh.DataSource = chiNhanhBUS.getToanBoSach();
                    Controller.isResetTb(TbChiNhanh, TbDiaChi);
                }
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin!", MessageDialogIcon.Error);
            }
        }

        // ................................................

        // Load form tương thích
        private void TcLibaryQuanLy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabDoiMatKhau)
            {
                TabDoiMatKhauAction();
            }    

            if (selectedTabPage == TabChiNhanh)
            {
                TabChiNhanhAction();
            }

            if (selectedTabPage == TabQlNhanVien)
            {
                TabQuanLyNhanVienAction();
            }    
        }

        // ................................................

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

        private bool isEmptys()
        {
            return Controller.isAllEmpty(TbTaiKhoan.Text, TbHoTen.Text, CbTrangThai.Text, TbEmail.Text, TbEmail.Text, CbGioiTinh.Text, CbGioiTinh.Text,
                CbHuyen.Text, CbXa.Text, DtpNgaySinh.Text);
        }

        private void BtnThemNhanVien_Click(object sender, EventArgs e)
        {
            string[] toEmail = { TbEmail.Text };
            string randomMatKhau = quanLyNguoiDungBUS.GenerateRandomPassword(13);
            string hashMatKhau = Controller.MD5Hash(randomMatKhau);

            if (isEmptys() && Controller.isEmpty(hashMatKhau))
            {
                Controller.isSendToEmails(toEmail, "eBook Gửi thông tin tài khoản nhân viên của bạn\U0001f970", ContentEmail.ctCreateNhanVien(TbHoTen.Text, TbTaiKhoan.Text, randomMatKhau));
                Controller.isAlert(MdQuanLy, "Vui lòng kiểm tra email", "Thông tin tài khoản đã gưi tới bạn✓", MessageDialogIcon.None);
                string diaChi = CbTinh.Text + "|" + CbHuyen.Text + "|" + CbXa.Text;
                int trangThai = (CbTrangThai.SelectedIndex == 0) ? 1 : -1;
                quanLyNguoiDungDTO.taiKhoan = TbTaiKhoan.Text;
                quanLyNguoiDungDTO.matKhau = hashMatKhau;
                quanLyNguoiDungDTO.hoTen = TbHoTen.Text;
                quanLyNguoiDungDTO.quyen = 1;
                quanLyNguoiDungDTO.trangThaiLamViec = trangThai;
                quanLyNguoiDungDTO.email = TbEmail.Text;
                quanLyNguoiDungDTO.gioiTinh = CbGioiTinh.Text;
                quanLyNguoiDungDTO.diaChi = diaChi;
                quanLyNguoiDungDTO.ngaySinh = DtpNgaySinh.Value;

                // Thêm nhân viên
                quanLyNhanVienBUS.insertNhanVien(quanLyNguoiDungDTO);

                DgvNhanVien.DataSource = quanLyNhanVienBUS.getDsNhanVien();
            }
            else
            {
                Controller.isAlert(MdQuanLy, "Không hợp lệ", "Vui lòng nhập đầy đủ thông tin", MessageDialogIcon.Error);
            }
        }

        private void BtnChinhSua_Click(object sender, EventArgs e)
        {
            if (isEmptys())
            {
                string diaChi = CbTinh.Text + "|" + CbHuyen.Text + "|" + CbXa.Text;
                int trangThai = (CbTrangThai.SelectedIndex == 0) ? 1 : -1;
                quanLyNguoiDungDTO.taiKhoan = TbTaiKhoan.Text;
                quanLyNguoiDungDTO.hoTen = TbHoTen.Text;
                quanLyNguoiDungDTO.quyen = 1;
                quanLyNguoiDungDTO.trangThaiLamViec = trangThai;
                quanLyNguoiDungDTO.email = TbEmail.Text;
                quanLyNguoiDungDTO.gioiTinh = CbGioiTinh.Text;
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
            if (Controller.isEmpty(TbTaiKhoan.Text))
            {
                if (quanLyNguoiDungBUS.checkAccountExists(TbTaiKhoan.Text))
                {
                    Controller.isAlert(MdQuanLy, "Xảy ra lỗi", "Tài khoản đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                    TbTaiKhoan.Text = "";
                }
            }    
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = DgvNhanVien.Rows[e.RowIndex];

                string hoTen = row.Cells["hoTen"].Value.ToString();
                string taiKhoan = row.Cells["taiKhoan"].Value.ToString();
                string email = row.Cells["email"].Value.ToString();
                string diaChi = row.Cells["nvDiaChi"].Value.ToString();
                string ngaySinh = row.Cells["ngaySinh"].Value.ToString();
                string gioiTinh = row.Cells["gioiTinh"].Value.ToString();
                string trangThai = row.Cells["trangThai"].Value.ToString();
                string ngayTao = row.Cells["nvNgayTao"].Value.ToString();

                TbHoTen.Text = hoTen; TbTaiKhoan.Text = taiKhoan; TbEmail.Text = email;
                string[] arrayDiaChi = diaChi.Split('|');
                CbTinh.Text = arrayDiaChi[0];
                CbHuyen.Text = arrayDiaChi[1];
                CbXa.Text = arrayDiaChi[2];

                DateTime ngaySinhRevert;
                if (DateTime.TryParse(ngaySinh, out ngaySinhRevert))
                {
                    DtpNgaySinh.Value = ngaySinhRevert;
                }
                CbGioiTinh.Text = gioiTinh;
                CbTrangThai.Text = trangThai;
            }
        }

        private void BtnHuyChon_Click(object sender, EventArgs e)
        {
            Controller.isResetTb(TbHoTen, TbTaiKhoan, TbEmail);
            CbTinh.SelectedIndex = 0;
            CbGioiTinh.SelectedIndex = 0;
            DtpNgaySinh.Value = DateTime.Now;
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

        private void TbEmail_Leave(object sender, EventArgs e)
        {
            if (Controller.isEmpty(TbEmail.Text))
            {
                if (!TbEmail.Text.Contains("@gmail.com") || TbEmail.Text.Length < "@gmail.com".Length)
                {
                    Controller.isAlert(MdQuanLy, "Không hợp lệ", "Email phải chứa '@gmail.com' và có từ 11 kí tự", MessageDialogIcon.Error);
                    TbEmail.Text = "";
                }
                else
                {
                    if (quanLyNguoiDungBUS.checkEmailExists(TbEmail.Text))
                    {
                        Controller.isAlert(MdQuanLy, "Xảy ra lỗi", "Email đã tồn tại trên hệ thống", MessageDialogIcon.Error);
                        TbEmail.Text = "";
                    }
                }
            }    
        }

        // ................................................
    }
}
