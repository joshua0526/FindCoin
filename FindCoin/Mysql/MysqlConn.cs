using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FindCoin.Mysql
{
    class MysqlConn
    {
        public static string conf = "";

        public static DataSet ExecuteDataSet(string sqltext) {
            using (MySqlConnection conn = new MySqlConnection(conf)) {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqltext, conf);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public static int ExecuteDataInsert(string tableName, List<string> parameter)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string mysql = $"insert into block values (null,";
                foreach (string param in parameter) {
                    mysql += "'" + param + "',";
                }               
                mysql = mysql.Substring(0, mysql.Length - 1);
                mysql += ");";
                MySqlCommand mc = new MySqlCommand(mysql, conn);
                int count = mc.ExecuteNonQuery();
                return count;
            }
        }

        /// <summary>
        /// 插入多条数据
        /// </summary>
        public static void InsertCollection(MySqlConnection connection)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = "INSERT INTO person VALUES ( null,?name, ?birthday)";
            command.Parameters.Add("?name", MySqlDbType.VarChar);
            command.Parameters.Add("?birthday", MySqlDbType.DateTime);

            for (int x = 0; x < 30; x++)
            {
                command.Parameters[0].Value = "name" + x;
                command.Parameters[1].Value = DateTime.Now;
                command.ExecuteNonQuery();
            }

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        private static int Update(MySqlConnection connection, string newName, string oldName)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand($"update person set name = '{newName}' where name = '{oldName}';", connection);
            int count = command.ExecuteNonQuery();
            connection.Close();
            return count;
        }
    }
}
