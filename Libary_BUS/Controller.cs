using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;
using System.Drawing;
using Guna.UI2.WinForms;
using System.Xml.Linq;
using System.Security.Permissions;
using System.Collections;
using Libary_Manager.Libary_DAO;
using System.Diagnostics;
using Libary_Manager.Libary_GUI.DoGia;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Libary_Manager.Libary_DTO;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using static DevExpress.LookAndFeel.DXSkinColors;

namespace Libary_Manager.Libary_BUS
{
    class Controller
    {
        // Đường dẫn tới ảnh sách 
        public static string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static string _PATH_PHOTO_BOOK = projectDirectory + @"\Photos\Books\";
        public static string _PATH_PHOTO_ITEM = projectDirectory + @"\Photos\Items\";
        private static string _PATH_PHOTOS_DELETED = projectDirectory + @"\PhotoDeleteds.txt";

        // Thêm các ảnh muốn xóa vào 
        public static string DeletedPhotos;

        // Số lượng bản ghi mỗi page
        public static int _MAX_PAGE = 6;

        // Kiểm tra giá trị có rỗng không 
        public static bool isEmpty(string value)
        {
            return (value.Trim() != "");
        }

        // kiểm tra độ dài có đủ
        public static bool isLength(string value, int length)
        {
            return value.Trim().Length >= length;
        }

        // Alert thông báo 
        public static void isAlert(Guna2MessageDialog alert, string caption, string text, MessageDialogIcon icon)
        {
            alert.Text = text;
            alert.Caption = caption;
            alert.Buttons = MessageDialogButtons.OK;
            alert.Icon = icon;
            alert.Style = MessageDialogStyle.Dark;
            alert.Show();
        }

        // Đặt câu hỏi cho người dùng
        public static bool isQuestion(Guna.UI2.WinForms.Guna2MessageDialog dialog, string caption, string text)
        {
            dialog.Text = text;
            dialog.Caption = caption;
            dialog.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            dialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            dialog.Style = Guna.UI2.WinForms.MessageDialogStyle.Dark;
            DialogResult result = dialog.Show();
            return result == DialogResult.Yes;
        }


        // Hiển thị việc chọn file cho người dùng
        public static OpenFileDialog isOpenfile(OpenFileDialog file, string type = "path")
        {
            if (file.ShowDialog() == DialogResult.OK)
            {
                return file;
            }
            return null;
        }

        // Kiểm tra dữ liệu trống sll
        public static bool isAllEmpty(params string[] values)
        {
            foreach (var value in values)
            {
                if (!isEmpty(value)) 
                    return false; 
            }
            return true; 
        }


        // Kiểm tra tên file tải lên 
        public static bool isExistsName(string pathImage)
        {
            if (File.Exists(pathImage))
            {
                MessageBox.Show("Tên ảnh đã tồn tại vui lòng đổi tên khác", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Lưu file vào folder  
        public static string isSavedFile(string fileName, string type)
        {
            try
            {
                string fileNameNew;
                string destinationPath;

                if (type == "book")
                {
                    fileNameNew = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(fileName);
                    destinationPath = Path.Combine(_PATH_PHOTO_BOOK, fileNameNew);
                    File.Copy(fileName, destinationPath, true);
                    return fileNameNew;
                }   
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra: " + ex.Message, "Lỗi không thể lưu!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Load dữ liệu 
        public static void isLoadDataPhoto(DataTable dataTable, DataGridView dataGridView, string namePhoto)
        {
            dataGridView.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = dataGridView.Rows.Add();
                for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
                {
                    string columnName = dataGridView.Columns[columnIndex].Name;
                    if (dataTable.Columns.Contains(columnName))
                    {
                        object cellValue = row[columnName];
                        if (columnName == namePhoto)
                        {
                            if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                            {
                                string imagePath = _PATH_PHOTO_BOOK + cellValue.ToString();
                                if (File.Exists(imagePath))
                                {
                                    System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
                                    dataGridView.Rows[rowIndex].Cells[columnIndex].Value = img;
                                }
                            }
                        }
                        else
                        {
                            dataGridView.Rows[rowIndex].Cells[columnIndex].Value = cellValue;
                        }
                    }
                }
            }
        }

        // Reset dữ liệu 
        public static void isResetTb(params Guna2TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
            }
        }

        // Xóa bỏ khi chương trình tắt
        public static void isDeletePhotos()
        {
            if (File.Exists(_PATH_PHOTOS_DELETED))
            {
                using (StreamWriter writer = new StreamWriter(_PATH_PHOTOS_DELETED))
                {
                    writer.Write(DeletedPhotos);
                }
            }    
        }

        // Tiến hành xóa bỏ file 
        public static void proceedDeletePhotos()
        {
            if (File.Exists(_PATH_PHOTOS_DELETED))
            {
                using (StreamReader reader = new StreamReader(_PATH_PHOTOS_DELETED))
                {
                    string content = reader.ReadToEnd();
                    string[] separationFileName = content.Split(';');
                
                    foreach (string file in separationFileName)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }    
                    }
                }
            }
        }

        // Trả về câu lệnh lấy số bản ghi phân trang
        public static string isHandlePagination(int page, int total)
        {
            int Offset;
            Offset = (page - 1) * total;
            return " OFFSET " + Offset + " ROWS FETCH NEXT " + total + " ROWS ONLY" ?? "";
        }

        // Gửi nhiều email 1 lúc 
        public static void isSendToEmails(string[] toEmails, string Title, string Content)
        {
            string fromEmail = "kothanhcong050@gmail.com";
            string subject = Title;
            string body = Content;
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("kothanhcong050@gmail.com", "qzpy mhln rhgf gpwu");
                    smtp.EnableSsl = true;

                    foreach (string toEmail in toEmails)
                    {
                        using (MailMessage mail = new MailMessage(fromEmail, toEmail))
                        {
                            mail.Subject = subject;
                            mail.Body = body;
                            mail.IsBodyHtml = true;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi gửi mail: " + ex.Message);
            }
        }


        // MD5 Mã hóa 
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        public static ArrayList GetThu2AndChuNhat()
        {
            ArrayList Thu2AndCn = new ArrayList();
            DateTime today = DateTime.Now;

            DateTime startOfWeek;
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = today.AddDays(-6);
            }
            else
            {
                startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            }
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // Chỉ lấy ngày tháng năm, không lấy giờ
            string startDate = startOfWeek.ToString("dd/MM/yyyy");
            string endDate = endOfWeek.ToString("dd/MM/yyyy");

            Thu2AndCn.Add(startDate);
            Thu2AndCn.Add(endDate);
            return Thu2AndCn;
        }

    }
}
