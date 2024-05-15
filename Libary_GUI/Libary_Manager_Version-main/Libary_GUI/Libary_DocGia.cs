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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Libary_Manager
{
    public partial class Libary_DocGia : Form
    {
        private int _PAGE = 1;

        private ArrayList objectSavedTabName = new ArrayList();

        private BUS_Sach sachBUS;

        private BUS_PhieuMuon phieuMuonBUS;

        // ................................................

        private DTO_Sach sachDTO;

        private DTO_PhieuMuon phieuMuonDTO;

        // ................................................

        public Libary_DocGia()
        {
            InitializeComponent();
        }

        // ................................................

        // Chọn sách thư viện
        void TabSachThuVienAction()
        {
            this.sachBUS = new BUS_Sach();
            this.sachDTO = new DTO_Sach();

            sachBUS.setTabControl(TcLibary);

            // Load toàn bộ danh sách Sách
            DataTable data = sachBUS.dataPagination(_PAGE);
            Controller.isLoadDataPhoto(data, DgvSachThuVien, "photo");
        }

        // Chọn đổi mật khẩu 
        void TabDoiMatKhauAction()
        {

        }

        // ................................................

        // Chọn phiếu mượn của tôi
        void TabPhieuMuonAction()
        {
            if (BUS_PhieuMuon.dictioLoanSlip.Count > 0)
            {
                BtnGuiPhieuMuon.Visible = true;

                this.phieuMuonBUS = new BUS_PhieuMuon();
                this.phieuMuonDTO = new DTO_PhieuMuon();

                DataTable data = phieuMuonBUS.getInfoPhieuSach();
                Controller.isLoadDataPhoto(data, DgvPhieuMuon, "pmPhoto");
            }    
        }

        // ................................................

        // Load form TrangChu
        private void Libary_TrangChu_Load(object sender, EventArgs e)
        {

        }

        // ................................................

        // Load form tương thích
        private void TcLibary_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTabPage = tabControl.SelectedTab;

            if (selectedTabPage == TabSachThuVien)
            {
                TabSachThuVienAction();
            }

            else if (selectedTabPage == TabDoiMatKhau)
            {
                TabDoiMatKhauAction();
            }

            else if (selectedTabPage == TabPhieuMuon)
            {
                TabPhieuMuonAction();
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

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // ................................................

        // Sách thư viện
    }
}
