﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Libary_Manager.Libary_BUS;
using Libary_Manager.Libary_DTO;

namespace Libary_Manager.Libary_GUI
{
    public partial class Libary_DangNhap : Form
    {
        private BUS_DangNhap dangNhapBUS;
        private DTO_DangNhap dangNhapDTO;

        public Libary_DangNhap()
        {
            InitializeComponent();
        }

        private void Libary_DangNhap_Load(object sender, EventArgs e)
        {

            this.dangNhapBUS = new BUS_DangNhap();
            this.dangNhapDTO = new DTO_DangNhap();

            TbTaiKhoan.Focus();
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setInfomation(DataTable data)
        {
            DTO_QuanLyNguoiDung.id = int.Parse(data.Rows[0]["id"].ToString());
            DTO_DangNhap.id = DTO_QuanLyNguoiDung.id;
            DTO_DangNhap.hoTen = data.Rows[0]["hoTen"].ToString();
            DTO_DangNhap.quyen = int.Parse(data.Rows[0]["quyen"].ToString());
            DTO_DangNhap.email = data.Rows[0]["email"].ToString();
            if (data.Rows[0]["mssv"].ToString() != "")
            {
                DTO_DangNhap.mssv = data.Rows[0]["mssv"].ToString();
            }
            if (data.Rows[0]["maChiNhanh"].ToString() != "")
            {
                DTO_DangNhap.maChiNhanh = int.Parse(data.Rows[0]["maChiNhanh"].ToString());
            }
            DTO_DangNhap.gioiTinh = data.Rows[0]["gioiTinh"].ToString();
            DTO_DangNhap.diaChi = data.Rows[0]["diaChi"].ToString();
            if (DateTime.TryParse(data.Rows[0]["ngaySinh"].ToString(), out DateTime parsedDate))
            {
                DTO_DangNhap.ngaySinh = parsedDate;
            }
      
            switch (DTO_DangNhap.quyen)
            {
                case 0:
                    Libary_QuanLy formQL = new Libary_QuanLy();
                    formQL.Show();
                    break;

                case 1:
                    Libary_NhanVien formNV = new Libary_NhanVien();
                    formNV.Show();
                    break;

                case 2:
                    Libary_DocGia formSV = new Libary_DocGia();
                    formSV.Show();
                    break;

                case 3:
                    Libary_DocGia formGV = new Libary_DocGia();
                    formGV.Show();
                    break;

                default: break;
            }

            PtLoadDing.Visible = false;
        }

        private void BtnDangNhap_Click_1(object sender, EventArgs e)
        {
            PtLoadDing.Visible = true;
            DTO_DangNhap.taiKhoan = TbTaiKhoan.Text;
            dangNhapDTO.matKhau = Controller.MD5Hash(TbMatKhau.Text);

            DataTable data = dangNhapBUS.checkDangNhap(dangNhapDTO);

            if (data.Rows.Count > 0)
            {
                int quyen = int.Parse(data.Rows[0]["quyen"].ToString());
      
                if (quyen == 1)
                {
                    if (dangNhapBUS.checkTrangThaiNhanVien())
                    {
                        Controller.isAlert(MdDangNhap, "Không hợp lệ", "Phiên hết hạn, tài khoàn đã bị loại bỏ!", MessageDialogIcon.Error);
                        return;
                    }
                    else
                    {
                        DateTime today = DateTime.Today;
                        DayOfWeek dayOfWeek = today.DayOfWeek;
                        DTO_ChamCong.thuMay = (int)(dayOfWeek + 1);
                    }
                }
                setInfomation(data);
            }
            else
            {
                Controller.isAlert(MdDangNhap, "Không hợp lệ", "Tài khoản hoặc mật khẩu sai", MessageDialogIcon.Error);
                PtLoadDing.Visible = false;
            }
        }

        private void TbTaiKhoan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '=')
            {
                e.Handled = true;
                Controller.isAlert(MdDangNhap, "Không hợp lệ" ,"Không được nhập dấu '='", MessageDialogIcon.Warning);
            }
        }

        private void TbMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '=')
            {
                e.Handled = true;
                Controller.isAlert(MdDangNhap, "Không hợp lệ", "Không được nhập dấu '='", MessageDialogIcon.Warning);
            }
        }

        private void Libary_DangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Controller.isDeletePhotos();
        }
    }
}