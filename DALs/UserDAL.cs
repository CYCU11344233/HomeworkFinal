using System.Data.OleDb;

namespace HomeworkFinal.DALs
{
    public class UserDAL
    {

        private readonly string _connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\CYCU11344233\HomeworkFinal\MyAccessDB.mdb";
        // 我筆電的ver
        //private readonly string _connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\hongw\source\repos\CYCU11344233\HomeworkFinal\MyAccessDB.mdb";
        public Boolean Authorize(string uid, string password)
        {
            Boolean result = false;
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                //string SqlStr = "select * from [Users] where [Name]=@uid and [Password]=@password";
                string SqlStr = "select * from [Users] where [Name]=? and [Password]=?";
                OleDbCommand cmd = new OleDbCommand(SqlStr, conn);
                cmd.Parameters.Add(new OleDbParameter("Name", uid));
                cmd.Parameters.Add(new OleDbParameter("Password", password));
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    { result = true; }
                    else
                    { result = false; }
                }
            }
            return result;
        }

        public bool Register(string uid, string password)
        {
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();

                // 1. 檢查帳號是否已存在
                string checkSql = "SELECT COUNT(*) FROM [Users] WHERE [Name]=?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkSql, conn))
                {
                    checkCmd.Parameters.Add(new OleDbParameter("Name", uid));
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0) return false; // 帳號重複，註冊失敗
                }

                // 2. 帳號沒人用，執行新增
                string insertSql = "INSERT INTO [Users] ([Name], [Password]) VALUES (?, ?)";
                using (OleDbCommand insertCmd = new OleDbCommand(insertSql, conn))
                {
                    insertCmd.Parameters.Add(new OleDbParameter("Name", uid));
                    insertCmd.Parameters.Add(new OleDbParameter("Password", password));

                    int affectedRows = insertCmd.ExecuteNonQuery();
                    return affectedRows > 0; // 回傳是否成功寫入
                }
            }
        }
    }
}
