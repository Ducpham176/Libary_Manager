using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_BUS.Content
{
    class ContentEmail
    {
        public static string ctCreateNhanVien(string hoTen, string taiKhoan, string matKhau)
        {
            return "<div style=\"width:500px;margin:auto;font-family:Arial,sans-serif;text-align:center;border:1px solid #e5e3dd;padding:30px 20px;box-sizing:border-box\"><h2 style=\"font-size:24px;color:#333\">🔓Thông tin tài khoản nhân viên " + hoTen + "</h2> <h2 style=\"font-size:24px;color:#333\"></h2> <p style=\"font-size:16px;color:#777\">Tài khoản này dùng đề đăng nhập và làm việc tại hệ thống bạn có thể truy cập và đổi mật khẩu</p> <p style=\"font-size:16px;color:#777\"></p> <div style=\"width:100%;background:#38bdf8;color:#fff;font-size:17px;padding:10px;box-sizing:border-box;border-radius:4px\">\r\n<p>Tài khoản: " + taiKhoan + "</p>\r\n<p>Mật khẩu: " + matKhau + "</p>\r\n</div> <p style=\"font-size:16px;color:#777\">eBook xin cảm ơn \U0001f970 </p> <div class=\"yj6qo\"></div><div class=\"adL\"> </div></div>";
        }

        public static string ctCreatenDocGia(string hoTen, string taiKhoan, string matKhau)
        {
            return "<div style=\"width:500px;margin:auto;font-family:Arial,sans-serif;text-align:center;border:1px solid #e5e3dd;padding:30px 20px;box-sizing:border-box\"><h2 style=\"font-size:24px;color:#333\">🔓Thông tin tài khoản độc giả " + hoTen + "</h2> <h2 style=\"font-size:24px;color:#333\"></h2> <p style=\"font-size:16px;color:#777\">Tài khoản này dùng đề đăng nhập và làm việc tại hệ thống bạn có thể truy cập và đổi mật khẩu</p> <p style=\"font-size:16px;color:#777\"></p> <div style=\"width:100%;background:#818cf8;color:#fff;font-size:17px;padding:10px;box-sizing:border-box;border-radius:4px\">\r\n<p>Tài khoản: " + taiKhoan + "</p>\r\n<p>Mật khẩu: " + matKhau + "</p>\r\n</div> <p style=\"font-size:16px;color:#777\">eBook xin cảm ơn \U0001f970 </p> <div class=\"yj6qo\"></div><div class=\"adL\"> </div></div>";
        }
    }
}
