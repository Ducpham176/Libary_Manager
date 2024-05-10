﻿using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_DTO;
using System;
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


        // Xác nhận đã load dữ liệu Dgv
        private bool valueChangeDgvSach = false;
        // Lưu tạm tên ảnh 
        private string namePhotoBook;
        private bool isClickPhoto = false;
        private string namePhotoPresent;
        // ____________________________________________________________

        private BUS_Sach sachBUS;
        private BUS_ChiNhanh chiNhanhBUS;

        // ____________________________________________________________

        private DTO_Sach sachDTO;
        private DTO_ChiNhanh chiNhanhDTO;

        // ____________________________________________________________

        public Libary_NhanVien()
        {
            InitializeComponent();
        }
        // ____________________________________________________________

        // Chọn quản lý sách sách
        void TabSachThuVienAction()
        {
            // Sách 
            this.sachBUS = new BUS_Sach();
            this.sachDTO = new DTO_Sach();

            this.chiNhanhBUS = new BUS_ChiNhanh();

            // Load toàn bộ danh sách, SÁCH

            DataTable data = sachBUS.getToanBoSach();
            Controller.isPhotoLoad(data, DgvSachThuVien);

            valueChangeDgvSach = true;

            // Load toàn bộ Chi nhánh vào thêm sách
            CbbChiNhanh.DataSource = chiNhanhBUS.getToanBoSach();
            CbbChiNhanh.DisplayMember = "chiNhanh";
            CbbChiNhanh.ValueMember = "chiNhanh";
        }

        // ____________________________________________________________

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

        // Nhấn delete xóa
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
                    MessageBox.Show("Đã up");
                }    
            }
        }

        public bool isEmptys()
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

                    // Load sách mới thêm
                    DataTable data = sachBUS.getToanBoSach();
                    Controller.isPhotoLoad(data, DgvSachThuVien);

                    // Reset value 
                    CbbChiNhanh.SelectedIndex = 0;
                    Controller.isResetTb(TbTuaSach, TbTacGia, TbNhaXuatBan, TbLoiGioiThieu, TbNamXuatBan, TbSoLuong);
                }
                catch (Exception)
                {
                    Controller.isAlert("Vui lòng nhập thông tin hợp lệ!", "Lỗi xảy ra", MessageBoxIcon.Error); return;
                }
                // Thêm sách
                sachBUS.insertSach(sachDTO);
            } 
            else
            {
                Controller.isAlert("Vui lòng nhập đầy đủ thông tin!", "Lỗi xảy ra", MessageBoxIcon.Error);
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
            sachDTO.tuaSach = TbTuaSach.Text;
            sachDTO.tacGia = TbTacGia.Text;
            sachDTO.nhaXuatBan = TbNhaXuatBan.Text;
            sachDTO.namXuatBan = TbNamXuatBan.Text;
            sachDTO.maChiNhanh = chiNhanhBUS.getMaChiNhanh(CbbChiNhanh.Text);
            sachDTO.loiGioiThieu = TbLoiGioiThieu.Text;
            sachDTO.soLuong = int.Parse(TbSoLuong.Text);

            // Cập nhật sửa
            sachBUS.updateSach(sachDTO);
        }


    }
}
