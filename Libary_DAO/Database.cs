using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libary_Manager.Libary_DAO
{
    class Database : Connect
    {
        public static DataTable read(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                Connect.Instance.GetConnection().Open();

                SqlDataAdapter query = new SqlDataAdapter(sql, Connect.Instance.GetConnection());
                DataTable table = new DataTable();
                query.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); 
                return null;
            }
            finally
            {
                Connect.Instance.GetConnection().Close();
            }
        }

        public static bool insert(string table, Dictionary<string, object> data)
        {
            try
            {
                Connect.Instance.GetConnection().Open();

                string keys = "(" + string.Join(", ", data.Keys) + ")";
                string values = "(" + string.Join(", ", data.Keys.Select(value => "@" + value)) + ")";
                string sql = "INSERT INTO " + table + keys + " VALUES " + values;

                SqlCommand cmd = new SqlCommand(sql, Connect.Instance.GetConnection());

                foreach (var item in data)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }

                return (cmd.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                Connect.Instance.GetConnection().Close();
            }
        }


        public static bool update(string table, Dictionary<string, object> data, string condition)
        {
            try
            {
                Connect.Instance.GetConnection().Open();

                string result = "";

                foreach (var pair in data)
                {
                    string path = "[" + pair.Key + "] = @" + pair.Key + ",";
                    result += path;
                }

                string query = "UPDATE " + table + " SET " + result.TrimEnd(',') + " WHERE " + condition;

                using (SqlCommand cmd = new SqlCommand(query, Connect.Instance.GetConnection()))
                {
                    foreach (var pair in data)
                    {
                        cmd.Parameters.AddWithValue("@" + pair.Key, pair.Value);
                    }

                    return (cmd.ExecuteNonQuery() > 0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                Connect.Instance.GetConnection().Close();
            }
        }


        public static bool delete(string table, string condition)
        {
            try
            {
                Connect.Instance.GetConnection().Open();

                string sql = "DELETE FROM " + table + " WHERE " + condition;

                SqlCommand cmd = new SqlCommand(sql, Connect.Instance.GetConnection());

                return (cmd.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                Connect.Instance.GetConnection().Close();
            }
        }
    }
}
