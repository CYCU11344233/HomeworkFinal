using System.Data.OleDb;

namespace HomeworkFinal.DALs
{
    public class UserDAL
    {
        private readonly string _connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\hongw\source\repos\CYCU11344233\HomeworkFinal\MyAccessDB.mdb";
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
    }
}
