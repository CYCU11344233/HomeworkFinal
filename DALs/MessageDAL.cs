using HomeworkFinal.Models;
using System.Data.OleDb;

namespace HomeworkFinal.DALs
{
    public class MessageDAL
    {

        private readonly string _connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\CYCU11344233\HomeworkFinal\MyAccessDB.mdb";
        // 我筆電的ver
        //private readonly string _connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\hongw\source\repos\CYCU11344233\HomeworkFinal\MyAccessDB.mdb";
        public bool ReleaseMessage(Message msg)
        {
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                // SQL 語句包含四個欄位
                string sql = "INSERT INTO [Messages] ([Subject], [Content], [Publisher], [PublishTime]) VALUES (?, ?, ?, ?)";

                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    // 參數順序必須跟 SQL 的 ? 一模一樣
                    cmd.Parameters.Add("Subject", OleDbType.VarWChar).Value = msg.Subject ?? (object)DBNull.Value;
                    cmd.Parameters.Add("Content", OleDbType.LongVarWChar).Value = msg.Content ?? (object)DBNull.Value;
                    cmd.Parameters.Add("Publisher", OleDbType.VarWChar).Value = msg.Publisher ?? (object)DBNull.Value;

                    // 關鍵點：明確指定為 Date 類型
                    cmd.Parameters.Add("PublishTime", OleDbType.Date).Value = msg.PublishTime;

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public List<Message> GetMessagesByUser(string userName)
        {
            List<Message> list = new List<Message>();
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM [Messages] WHERE [Publisher] = ? ORDER BY [PublishTime] DESC";

                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OleDbParameter("Publisher", userName));

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Message
                            {
                                Subject = reader["Subject"].ToString(),
                                Content = reader["Content"].ToString(),
                                Publisher = reader["Publisher"].ToString(),
                                PublishTime = Convert.ToDateTime(reader["PublishTime"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
