using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_QuanLyNguoiDung
    {
        public DataTable getDsTinh()
        {
            try
            {
                string sql = "SELECT code, full_name FROM TV_TinhThanhPho";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getDsHuyen(string code)
        {
            try
            {
                string sql = "SELECT code, full_name FROM TV_QuanHuyen WHERE province_code = '" + code + "'";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable getDsXa(string code)
        {
            try
            {
                string sql = "SELECT full_name FROM TV_PhuongXa WHERE district_code = '" + code + "'";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public int checkEmailExists(string email)
        {
            try
            {
                string sql = "SELECT id FROM TV_NguoiDung WHERE email = '" + email + "'";
                return Database.read(sql).Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        public int checkAccountExists(string account)
        {
            try
            {
                string sql = "SELECT id FROM TV_NguoiDung WHERE taiKhoan = '" + account + "'";
                return Database.read(sql).Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
    }
}
