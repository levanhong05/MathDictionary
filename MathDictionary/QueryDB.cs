using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;

namespace MathDictionary
{
    internal class QueryDB
    {
        public static OleDbConnection DbConnection;
        public static string szDataPath;
        public static string szDBDate = null;
        public static string szTemp_DBDate = null;

        public QueryDB()
        {
            QueryDB.szDataPath = Application.StartupPath + "\\MathDictionary.MDB";
            try
            {
                string str = char.ConvertFromUtf32(42) + 101.ToString() + char.ConvertFromUtf32(35);
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + QueryDB.szDataPath + ";Persist Security Info=False;Jet OLEDB:Database Password=" + str;
                QueryDB.DbConnection = new OleDbConnection(connectionString);
                QueryDB.DbConnection.Open();
                OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader("SELECT Value FROM Generals WHERE Name='LastUpdate'");
                if (oleDbDataReader.Read())
                {
                    QueryDB.szDBDate = oleDbDataReader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Process.GetCurrentProcess().Kill();
            }
        }
        public void DbClose()
        {
            QueryDB.DbConnection.Close();
        }
        public static OleDbDataReader DBExecuteReader(string szSQL)
        {
            OleDbCommand oleDbCommand = new OleDbCommand(szSQL, QueryDB.DbConnection);
            return oleDbCommand.ExecuteReader();
        }
        public static int DBExecuteNonQuery(string szSQL)
        {
            OleDbCommand oleDbCommand = new OleDbCommand(szSQL, QueryDB.DbConnection);
            return oleDbCommand.ExecuteNonQuery();
        }
        public void LoadKeys_byGroup(TreeView treeNode)
        {
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader("SELECT GroupVoca, Name FROM Vocabulary GROUP BY GroupVoca, Name");
            string b = "";
            int num = -1;
            int num2 = 0;
            while (oleDbDataReader.Read())
            {
                string @string = oleDbDataReader.GetString(0);
                if (@string != b)
                {
                    treeNode.Nodes.Add(oleDbDataReader.GetString(0));
                    b = oleDbDataReader.GetString(0);
                    num++;
                }
                treeNode.Nodes[num].Nodes.Add(oleDbDataReader.GetString(1));
                if (++num2 % 500 == 0)
                {
                    Application.DoEvents();
                }
            }
        }
        public void LoadKeys_byAlphabet(ListBox lstKeys, string szTable)
        {
            string szSQL = "SELECT Name FROM " + szTable + " ORDER BY Name";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            int num = 0;
            while (oleDbDataReader.Read())
            {
                lstKeys.Items.Add(oleDbDataReader[0].ToString());
                if (++num % 500 == 0)
                {
                    Application.DoEvents();
                }
            }
        }
    }
}
