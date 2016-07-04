using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MathDictionary
{
    internal class CImportExport
    {
        public int ExportToXml(string szName, string szTable, string szFilePath)
        {
            string selectCommandText = string.Concat(new string[]
			{
				"SELECT * FROM ",
				szTable,
				" WHERE ",
				szTable,
				".Name='",
				szName,
				"'"
			});
            try
            {
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, QueryDB.DbConnection);
                DataSet dataSet = new DataSet();
                dataSet.DataSetName = "Export";
                oleDbDataAdapter.Fill(dataSet, szTable);
                dataSet.WriteXml(szFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return 0;
            }
            return 1;
        }
        public int ExportOwnerToXml(string szFilePath)
        {
            string[] array = new string[]
			{
				"Vocabulary",
				"Formula",
				"Constants"
			};
            int num = 0;
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.DataSetName = "Export";
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    string selectCommandText = "SELECT * FROM " + text + " WHERE Owner LIKE('#%')";
                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, QueryDB.DbConnection);
                    num += oleDbDataAdapter.Fill(dataSet, text);
                }
                dataSet.WriteXml(szFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return -1;
            }
            return num;
        }
        public int ImportFromXmlToDB(string szFilePath)
        {
            int result;
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(szFilePath);
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    foreach (DataRow dataRow in dataSet.Tables[i].Rows)
                    {
                        string szSQL = string.Concat(new string[]
						{
							"SELECT * FROM ",
							dataSet.Tables[i].TableName,
							" WHERE Name='",
							dataRow["Name"].ToString(),
							"'"
						});
                        OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
                        bool flag = oleDbDataReader.Read();
                        oleDbDataReader.Close();
                        string text = "";
                        for (int j = 0; j < dataSet.Tables[i].Columns.Count; j++)
                        {
                            DataColumn dataColumn = dataSet.Tables[i].Columns[j];
                            if (j > 0)
                            {
                                text += ", ";
                            }
                            string text3="";
                            if (flag)
                            {
                                string text2 = text;
                                if (dataColumn.ColumnName == "LastEdit")
                                {
                                    text3 = dataRow[dataColumn.ColumnName].ToString().Split(new char[]
				                    {
					                    'T'
				                    })[0];
                                }
                                text = string.Concat(new string[]
								{
									text2,
									dataSet.Tables[i].TableName,
									".",
									dataColumn.ColumnName,
									"='",
									(dataColumn.ColumnName != "LastEdit" ? dataRow[dataColumn.ColumnName].ToString() : text3),
									"'"
								});
                            }
                            else
                            {
                                text = text + "'" + (dataColumn.ColumnName != "LastEdit" ? dataRow[dataColumn.ColumnName].ToString() : text3) + "'";
                            }
                        }
                        text = text.Replace("'#", "'");
                        string szSQL2;
                        if (flag)
                        {
                            szSQL2 = string.Concat(new string[]
							{
								"UPDATE ",
								dataSet.Tables[i].TableName,
								" SET ",
								text,
								" WHERE Name='",
								dataRow["Name"].ToString(),
								"'"
							});
                        }
                        else
                        {
                            szSQL2 = string.Concat(new string[]
							{
								"INSERT INTO ",
								dataSet.Tables[i].TableName,
								" VALUES(",
								text,
								")"
							});
                        }
                        QueryDB.DBExecuteNonQuery(szSQL2);
                    }
                }
                result = dataSet.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                result = 0;
            }
            return result;
        }
    }
}