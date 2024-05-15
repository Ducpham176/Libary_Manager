using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            this.dangNhapBUS = new BUS_DangNhap();
            this.dangNhapDTO = new DTO_DangNhap();
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            dangNhapDTO.taiKhoan = TbTaiKhoan.Text;
            dangNhapDTO.matKhau = TbMatKhau.Text;
            dangNhapBUS.checkDangNhap(dangNhapDTO);
        }
    }
}
