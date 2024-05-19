﻿using Libary_Manager.Libary_DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO.DAO_QuanLy
{
    class DAO_QuanLyDocGia
    {
        public DataTable getDsDocGia()
        {
            try
            {
                string sql = "SELECT dg.id, TRIM(dg.hoTen) AS hoTen, TRIM(dg.taiKhoan) AS taiKhoan, TRIM(dg.email) AS email, " +
                    "TRIM(dg.mssv) AS mssv, dg.gioiTinh, TRIM(dg.diaChi) AS diaChi, dg.ngaySinh, dg.ngayTao " +
                    "FROM TV_NguoiDung AS dg WHERE quyen = 2 ORDER BY dg.id DESC";
                return Database.read(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool insertDocGia(DTO_QuanLyNguoiDung quanLyDocGiaDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "taiKhoan", quanLyDocGiaDTO.taiKhoan },
                    { "matKhau", quanLyDocGiaDTO.matKhau },
                    { "hoTen", quanLyDocGiaDTO.hoTen },
                    { "quyen", quanLyDocGiaDTO.quyen },
                    { "email", quanLyDocGiaDTO.email },
                    { "mssv", quanLyDocGiaDTO.mssv },
                    { "gioiTinh", quanLyDocGiaDTO.gioiTinh },
                    { "diaChi", quanLyDocGiaDTO.diaChi },
                    { "ngaySinh", quanLyDocGiaDTO.ngaySinh },
                    { "ngayTao", DateTime.Now },
                };
                Database.insert("TV_NguoiDung", data); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool updateDocGia(DTO_QuanLyNguoiDung quanLyDocGiaDTO)
        {
            try
            {
                var data = new Dictionary<string, object>()
                {
                    { "hoTen", quanLyDocGiaDTO.hoTen },
                    { "quyen", quanLyDocGiaDTO.quyen },
                    { "gioiTinh", quanLyDocGiaDTO.gioiTinh },
                    { "diaChi", quanLyDocGiaDTO.diaChi },
                    { "ngaySinh", quanLyDocGiaDTO.ngaySinh },
                };
                string condition = " taiKhoan = '" + quanLyDocGiaDTO.taiKhoan + "'";
                Database.update("TV_NguoiDung", data, condition); return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi databse " + ex.Message, "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}