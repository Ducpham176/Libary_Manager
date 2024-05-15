using Libary_Manager.Libary_DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS
{
    class BUS_QuanLyNguoiDung
    {
        private DAO_QuanLyNguoiDung quanLyNguoiDungDAO;

        public BUS_QuanLyNguoiDung()
        {
            this.quanLyNguoiDungDAO = new DAO_QuanLyNguoiDung();
        }

        public string GenerateRandomPassword(int length)
        {
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "0123456789";
            char[] chars = new char[length];
            Random rd = new Random();
            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }
            }
            return new string(chars);
        }

        public DataTable getDsTinh()
        {
            try
            {
                return quanLyNguoiDungDAO.getDsTinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi lấy dữ liệu tỉnh: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable getDsHuyen(string code)
        {
            try
            {
                return quanLyNguoiDungDAO.getDsHuyen(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi lấy dữ liệu huyện: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable getDsXa(string code)
        {
            try
            {
                return quanLyNguoiDungDAO.getDsXa(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi lấy dữ liệu tỉnh: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public bool checkEmailExists(string email)
        {
            try
            {
                if (quanLyNguoiDungDAO.checkEmailExists(email) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi kiểm tra email: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }

        public bool checkAccountExists(string account)
        {
            try
            {
                if (quanLyNguoiDungDAO.checkAccountExists(account) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra khi kiểm tra tài khoản: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }
    }
}
