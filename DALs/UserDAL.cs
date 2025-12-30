using System.Data.OleDb;

namespace HomeworkFinal.DALs
{
    public class UserDAL : BaseDAL
    {
        public Boolean Authorize(string uid, string password)
        {
            Boolean result = false;
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
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

        public bool Register(string uid, string password, string gmail)
        {
            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();

                // 1. 檢查帳號是否已存在
                string checkSql = "SELECT COUNT(*) FROM [Users] WHERE [Name]=?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkSql, conn))
                {
                    checkCmd.Parameters.Add(new OleDbParameter("@Name", OleDbType.VarChar)).Value = uid;

                    // 使用 Convert.ToInt32 避免直接強轉失敗
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0) return false;
                }

                // 2. 執行新增
                string insertSql = "INSERT INTO [Users] ([Name], [Password], [Gmail]) VALUES (?, ?, ?)";
                using (OleDbCommand insertCmd = new OleDbCommand(insertSql, conn))
                {
                    // 嚴謹一點可以指定資料型別
                    insertCmd.Parameters.Add(new OleDbParameter("@Name", OleDbType.VarChar)).Value = uid;
                    insertCmd.Parameters.Add(new OleDbParameter("@Password", OleDbType.VarChar)).Value = password;
                    insertCmd.Parameters.Add(new OleDbParameter("@Gmail", OleDbType.VarChar)).Value = gmail;

                    int affectedRows = insertCmd.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }
    }
}
