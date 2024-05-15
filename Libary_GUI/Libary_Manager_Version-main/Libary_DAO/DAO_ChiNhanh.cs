using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class DAO_ChiNhanh
    {
        public DataTable readChiNhanh()
        {
            string sql = "SELECT id, TRIM(chiNhanh) as chiNhanh, TRIM(diaChi) as diaChi, ngayThem " +
                "FROM TV_ChiNhanh";
            return Database.read(sql);
        }

        public bool insertChiNhanh(DTO_ChiNhanh chiNhanhDTO)
        {
            var data = new Dictionary<string, object>()
            {
                { "chiNhanh", chiNhanhDTO.chiNhanh },
                { "diaChi", chiNhanhDTO.diaChi },
                { "ngayThem", chiNhanhDTO.ngayThem }
            };
            try
            {
                Database.insert("Tv_ChiNhanh", data); return true;
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool deleteChiNhanh(DTO_ChiNhanh chiNhanhDTO)
        {
            try
            {
                string condition = "id = '" + chiNhanhDTO.id + "'";
                Database.delete("Tv_ChiNhanh", condition); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable getMaChiNhanh(string chiNhanh)
        {
            try
            {
                string sql = "SELECT id FROM TV_ChiNhanh WHERE chiNhanh = N'" + chiNhanh + "'";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }    
    }
}
