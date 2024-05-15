using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_DTO;
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

namespace Libary_Manager.Libary_GUI
{
    public partial class Libary_NhanVien : Form
    {
        int _PAGE = 1;

        // Lưu tạm tên ảnh 
        private bool isClickPhoto = false;
        private string namePhotoBook;
        private string namePhotoPresent;

        // ................................................

        private BUS_Sach sachBUS;
        private BUS_ChiNhanh chiNhanhBUS;

        // ................................................

        private DTO_Sach sachDTO;

        // ................................................

        public Libary_NhanVien()
        {
            InitializeComponent();
        }

        // ................................................

        // Chọn quản lý sách sách
        void TabSachThuVienAction()
        {
            // Sách 
            this.sachBUS = new BUS_Sach();
            this.sachDTO = new DTO_Sach();

            this.chiNhanhBUS = new BUS_ChiNhanh();

            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");

            // Load toàn bộ Chi nhánh vào thêm sách
            CbbChiNhanh.DataSource = chiNhanhBUS.getToanBoSach();
            CbbChiNhanh.DisplayMember = "HoTen";
            CbbChiNhanh.ValueMember = "HoTen";
        }

        // ................................................

        // Chọn quản lý nhân viên 

        void TabQuanLyNhanVienAction()
        {

        }

        // Load form tương thích
        private void TcLibaryQuanLy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabSachThuVien)
            {
                TabSachThuVienAction();
            }
        }

        private void DgvSachThuVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                MessageBox.Show(sachDTO.maSach);
                if (sachBUS.deleteSach(sachDTO)) { };
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
                        string pathImage = sachBUS.prepareDeletePhoto(sachDTO);
                        Controller.DeletedPhotos.Add(pathImage);
                        sachDTO.photo = Controller.isSavedFile(namePhotoPresent, "book");
                    }
                    sachDTO.soLuong = int.Parse(TbSoLuong.Text);

                    // Cập nhật sửa
                    sachBUS.updateSach(sachDTO);

                    // Load sách mới chỉnh
                    DataTable data = sachBUS.getToanBoSach();
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

        private void Libary_NhanVien_FormClosed(object sender, FormClosedEventArgs e)
        {
            Controller.isDeletePhotos();
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
    }
}

