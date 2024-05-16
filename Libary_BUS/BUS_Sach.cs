using Guna.UI2.WinForms;
using Libary_Manager.Libary_DAO;
using Libary_Manager.Libary_DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_BUS
{
    class BUS_Sach 
    {
        // Phân trang thư viện 
        public static int _TOTAL_BOOK = -1;

        private DAO_Sach sachDAO;

        public static string maSachDetail;

        public static ArrayList arrayTabDetail = new ArrayList();

        public static TabControl tabControl = new TabControl();

        public BUS_Sach()
        {
            this.sachDAO = new DAO_Sach();
        }

        public void setMaSach(string maSach)
        {
            maSachDetail = maSach;
        }

        public string getMaSach()
        {
            return maSachDetail;
        }

        public void setObjectSavedTabName(ArrayList array)
        {
            arrayTabDetail = array;
        }

        public ArrayList getObjectSavedTabName()
        {
            return arrayTabDetail;
        }

        public void setTabControl(TabControl tabTC)
        {
            tabControl = tabTC;
        }    

        public TabControl getTabControl()
        {
            return tabControl;
        }

        public DataTable getToanBoSach()
        {
            return sachDAO.readSach();
        }

        public DataTable readSachEqualMaSach(string maSach)
        {
            try
            {
                if (sachDAO.readSachEqualMaSach(maSach) != null)
                {
                    return sachDAO.readSachEqualMaSach(maSach);
                }
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy dữ liệu sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public string createMaSach()
        {
            try
            {
                DataTable data = sachDAO.createMaSach();
                if (data.Rows.Count != 0)
                {
                    string maSach = data.Rows[0][0].ToString();
                    int numberMaSach = int.Parse(maSach.Substring(4)) + 1;
                    return "SDTA" + numberMaSach;
                } else
                {
                    return "SDTA0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo mới sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "SDTA0";
            };
        }

        public bool deleteSach(DTO_Sach sachDTO)
        {
            try
            {
                sachDAO.deleteSach(sachDTO); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }

        public bool insertSach(DTO_Sach sachDTO)
        {
            try
            {
                sachDAO.insertSach(sachDTO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }

        public bool updateSach(DTO_Sach sachDTO)
        {
            try
            {
                sachDAO.updateSach(sachDTO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chỉnh sửa sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
        }


        public string prepareDeletePhoto(DTO_Sach sachDTO)
        {
            try
            {
                string path = Controller._PATH_PHOTO_BOOK + sachDAO.getInfoBasedMaSach(sachDTO);
                return path;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chỉnh sửa sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable dataPagination(int page)
        {
            try
            {
                if (_TOTAL_BOOK == -1)
                {
                    _TOTAL_BOOK = sachDAO.getRows();
                }    
                string offset = Controller.isHandlePagination(page, Controller._MAX_PAGE);

                return sachDAO.dataPagination(offset);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable dataFullPagination(int page)
        {
            try
            {
                if (_TOTAL_BOOK == -1)
                {
                    _TOTAL_BOOK = sachDAO.getRows();
                }
                string offset = Controller.isHandlePagination(page, Controller._MAX_PAGE);

                return sachDAO.dataFullPagination(offset);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }

        public DataTable dataSearchBooks(string keyWord)
        {
            try
            {
                return sachDAO.dataSearchBooks(keyWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra trong tìm kiếm sách: " + ex.Message, "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            };
        }
    }
}
