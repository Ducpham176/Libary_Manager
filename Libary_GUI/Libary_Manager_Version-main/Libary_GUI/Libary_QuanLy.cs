using Libary_Manager.Libary_BUS;
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
        // Xác nhận đã load dữ liệu Dgv
        private bool valueChangeDgvSach = false;

        // ................................................

        private BUS_Sach sachBUS;
        private BUS_ChiNhanh chiNhanhBUS;

        // ................................................

        private DTO_Sach sachDTO;
        private DTO_ChiNhanh chiNhanhDTO;

        // ................................................

        public Libary_QuanLy()
        {
            InitializeComponent();
        }

        // ................................................

        // Chọn quản lý chi nhánh 
        void TabChiNhanhAction()
        {
            // Chí nhánh 
            this.chiNhanhBUS = new BUS_ChiNhanh();
            this.chiNhanhDTO = new DTO_ChiNhanh();

            DgvChiNhanh.DataSource = chiNhanhBUS.getToanBoSach();
        }

        // Chọn quản lý sách sách
        void TabSachThuVienAction()
        {
            // Sách 
            this.sachBUS = new BUS_Sach();
            this.sachDTO = new DTO_Sach();

            this.chiNhanhBUS = new BUS_ChiNhanh();
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
                }
            }
            else
            {
                Controller.isAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin!", MessageBoxIcon.Error);
            }
        }

        // ................................................

        // Load form tương thích
        private void TcLibaryQuanLy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabChiNhanh)
            {
                TabChiNhanhAction();
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

        // ................................................
    }
}
