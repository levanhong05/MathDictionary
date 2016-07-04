using System;
using System.Data.OleDb;
using System.Windows.Forms;
namespace MathDictionary
{
    internal class CModifyItem
    {
        public static string GetFormulaStr(string szName)
        {
            CDisplayInfo cDisplayInfo = new CDisplayInfo();
            string szSQL = "SELECT * FROM Formula WHERE Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            string result="";
            if (oleDbDataReader.Read())
            {
                string text = oleDbDataReader[0].ToString();
                string text2 = oleDbDataReader[1].ToString();
                string text3 = oleDbDataReader[2].ToString();
                string text4 = oleDbDataReader[4].ToString();
                string text5 = oleDbDataReader[5].ToString().Replace("#", "");

                result = string.Concat(new string[]
				{
					text,
					"¦",
					text2,
					"¦",
					text3,
					"¦",
					text4,
					"¦",
					text5,
				});
            }
            return result;
        }
        public static string GetVocabularyStr(string szName)
        {
            CDisplayInfo cDisplayInfo = new CDisplayInfo();
            string szSQL = "SELECT * FROM Vocabulary WHERE Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            string result = "";
            if (oleDbDataReader.Read())
            {
                string text = oleDbDataReader[0].ToString();
                string text2 = oleDbDataReader[1].ToString();
                string text3 = oleDbDataReader[2].ToString();
                string text4 = oleDbDataReader[3].ToString();
                string text5 = oleDbDataReader[4].ToString();
                string text6 = oleDbDataReader[5].ToString();
                string text7 = oleDbDataReader[6].ToString().Replace("#", "");
               
                result = string.Concat(new string[]
				{
					text,
					"¦",
					text2,
					"¦",
					text3,
					"¦",
					text4,
					"¦",
					text5,
					"¦",
					text6,
					"¦",
					text7
				});
            }
            return result;
        }
        public static string GetConstantStr(string szName)
        {
            new CDisplayInfo();
            string szSQL = "SELECT * FROM Constants WHERE Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            if (oleDbDataReader.Read())
            {
                string text = oleDbDataReader[0].ToString();
                string text2 = oleDbDataReader[1].ToString();
                string text3 = oleDbDataReader[2].ToString().Replace("#", "");
                if (!frmMain.bIsLanguage)
                {
                    text2 = text2.Replace("0x", "&H");
                }
                return string.Concat(new string[]
				{
					text,
					"¦",
					text2,
					"¦",
					text3
				});
            }
            return "";
        }

        public static int DeleteFromDB(string szKeyName, string szTable)
        {
            string szSQL = string.Concat(new string[]
			{
				"DELETE FROM ",
				szTable,
				" WHERE Name = '",
				szKeyName,
				"'"
			});
            return QueryDB.DBExecuteNonQuery(szSQL);
        }
        public static string UpdateVocabulary(string szKey, string szData)
        {
            string result = "";
            string[] array = szData.Split(new char[]
			{
				'¦'
			});
            if (array[0] == "" || array[1] == "")
            {
                MessageBox.Show("Word  or meaning not null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return result;
            }
            
            string text2 = DateTime.Now.Date.ToShortDateString();
            bool flag = false;
            if (szKey.StartsWith("Add New"))
            {
                string szSQL = "SELECT Name FROM Vocabulary WHERE Name = '" + array[0] + "'";
                OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
                if (oleDbDataReader.Read())
                {
                    MessageBox.Show("'" + array[0] + "' existed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return result;
                }
                flag = true;
            }
            string szSQL2;
            if (flag)
            {
                string text3 = "";
                string text4 = "";
                if (frmMain.bIsLanguage)
                {
                    text4 = array[5];
                }
                else
                {
                    text3 = array[5];
                }
                szSQL2 = string.Concat(new string[]
				{
					"INSERT INTO Vocabulary VALUES('",
					array[0],
					"','",
					array[1],
					"','",
					array[2],
					"','",
					array[3],
					"','",
					array[4],
					"','",
					array[5],
					"','#",
					array[6],
					"','",
					text2,
					"')"
				});
                result = "+" + array[0];
            }
            else
            {
                string text5 = "', " + (frmMain.bIsLanguage ? "Anh-Việt" : "Anh-Anh") + " = '";
                szSQL2 = string.Concat(new string[]
				{
					"UPDATE Vocabulary SET Vocabulary.Meaning = '",
					array[1],
					"', Description = '",
					array[2],
					"', Example = '",
					array[3],
					"', Solution = '",
					array[4],
					"', GroupVoca = '",
					array[5],
					"', Owner = '#",
					array[6],
					"', Name = '",
					array[0],
					"', LastEdit = '",
					text2,
					"' WHERE Name = '",
					szKey,
					"'"
				});
                result = array[0];
            }
            int num = QueryDB.DBExecuteNonQuery(szSQL2);
            if (num > 0)
            {
                MessageBox.Show("Success !", "Result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return result;
        }
        public static string UpdateFormula(string szKey, string szData)
        {
            string result = "";
            string[] array = szData.Split(new char[]
			{
				'¦'
			});
            if (array[0] == ""||array[1]=="")
            {
                MessageBox.Show("Formula name and value is not null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return result;
            }
            
            string text2 = DateTime.Now.Date.ToShortDateString();
            bool flag = false;
            if (szKey.StartsWith("Add New"))
            {
                string szSQL = "SELECT Name FROM Formula WHERE Name = '" + array[0] + "'";
                OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
                if (oleDbDataReader.Read())
                {
                    MessageBox.Show("'" + array[0] + "' existed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return result;
                }
                flag = true;
            }
            string szSQL2;
            if (flag)
            {
                szSQL2 = string.Concat(new string[]
				{
					"INSERT INTO Formula VALUES('",
					array[0],
					"','",
					array[1],
                    "','",
					array[2],
                    "','",
					array[3],
					"','#",
					array[4],
					"','",
					text2,
					"')"
				});
                result = "+" + array[0];
            }
            else
            {
                szSQL2 = string.Concat(new string[]
				{
					"UPDATE Formula SET Formula.Value = '",
					array[1],
                     "', Parameter = '",
					array[2],
                    "', GroupFormu = '",
					array[3],
					"', Owner = '#",
					array[4],
					"', Name = '",
					array[0],
					"', LastEdit = '",
					text2,
					"' WHERE Name = '",
					szKey,
					"'"
				});
                result = array[0];
            }
            int num = QueryDB.DBExecuteNonQuery(szSQL2);
            if (num > 0)
            {
                MessageBox.Show("Success !", "Result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return result;
        }
        public static string UpdateConstant(string szKey, string szData)
        {
            string result = "";
            string[] array = szData.Split(new char[]
			{
				'¦'
			});
            if (array[0] == "" || array[1] == "")
            {
                MessageBox.Show("Const name or value not null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return result;
            }
            if (!frmMain.bIsLanguage)
            {
                array[1] = array[1].Replace("&H", "0x");
            }
            string text = DateTime.Now.Date.ToShortDateString();
            bool flag = false;
            if (szKey.StartsWith("Add New"))
            {
                string szSQL = "SELECT Name FROM Constants WHERE Name = '" + array[0] + "'";
                OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
                if (oleDbDataReader.Read())
                {
                    MessageBox.Show("'" + array[0] + "' existed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return result;
                }
                flag = true;
            }
            string szSQL2;
            if (flag)
            {
                szSQL2 = string.Concat(new string[]
				{
					"INSERT INTO Constants VALUES('",
					array[0],
					"','",
					array[1],
					"','#",
					array[2],
					"','",
					text,
					"')"
				});
                result = "+" + array[0];
            }
            else
            {
                szSQL2 = string.Concat(new string[]
				{
					"UPDATE Constants SET Constants.Value = '",
					array[1],
					"', Owner = '#",
					array[2],
					"', Name = '",
					array[0],
					"', LastEdit = '",
					text,
					"' WHERE Name = '",
					szKey,
					"'"
				});
                result = array[0];
            }
            int num = QueryDB.DBExecuteNonQuery(szSQL2);
            if (num > 0)
            {
                MessageBox.Show("Success !", "Result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return result;
        }
    }
}