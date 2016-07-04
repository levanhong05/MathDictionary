using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
namespace MathDictionary
{
    internal class CSearch
    {
        private const int MAX_RECORD = 20;
        private const int MAX_LENGTH = 150;
        private string SQL_STATEMENT;
        private string szKeyword = "";
        private string szCategory = "";
        private int nTotalItem;
        public string DisplaySearchResults(int nPageId)
        {
            if (this.nTotalItem == 0)
            {
                return this.DisplaySearchResultsFailure(this.szKeyword, "", "");
            }
            int num = (nPageId - 1) * 20;
            int num2 = (num + 20 > this.nTotalItem) ? this.nTotalItem : (num + 20);
            string text = "<p style='margin-top: 5; margin-bottom: 5'><font face='Verdana' size='2'>";
            object obj = text;
            text = string.Concat(new object[]
			{
				obj,
				"Results <b>",
				num + 1,
				"-",
				num2,
				"</b> of about <b>",
				this.nTotalItem.ToString(),
				"</b> for: <b>"
			});
            text = text + "<font color=red>" + this.szKeyword + "</b></font></font></p><br>\n";
            int num3 = 0;
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(this.SQL_STATEMENT);
            while (oleDbDataReader.Read() && num3++ < num2)
            {
                if (num3 > num)
                {
                    string text2 = "";
                    if (this.szCategory == "Vocabulary")
                    {
                        int[] array = new int[]
						{
							2,
							3,
							4,
						};
                        int num4 = 0;
                        while (num4 < 3 && text2.Length < 150)
                        {
                            string text3 = oleDbDataReader[array[num4]].ToString();
                            if (text3 != "")
                            {
                                text2 = text2 + text3 + " ";
                            }
                            num4++;
                        }
                        if (text2.Length > 150)
                        {
                            text2 = text2.Substring(0, 150) + " ... ";
                        }
                        else
                        {
                            if (text2.Length < 5)
                            {
                                text2 = "No description available.";
                            }
                        }
                    }
                    if (this.szCategory == "Constants")
                    {
                        text2 = oleDbDataReader[1].ToString();
                    }
                    text += this.FormatSearchItem(num3, oleDbDataReader[0].ToString(), text2);
                }
            }
            text += "<p style='margin-top: 5; margin-bottom: 5' align='center'><font face=Verdana size=2>\n";
            string text4 = "";
            int num5 = nPageId - 1;
            while (num5 > nPageId - 3 && num5 > 0)
            {
                text4 = this.HTML_VIEWPAGE(num5, "[" + num5.ToString() + "]") + text4;
                num5--;
            }
            if (nPageId > 1)
            {
                text4 = this.HTML_VIEWPAGE(1, "First") + this.HTML_VIEWPAGE(nPageId - 1, "Prev") + text4;
            }
            text4 = text4 + "<font color=red>[" + nPageId.ToString() + "]</font>&nbsp;&nbsp;";
            int num6 = this.nTotalItem / 20 + ((this.nTotalItem % 20 > 0) ? 1 : 0);
            int num7 = nPageId + 1;
            while (num7 < nPageId + 3 && num7 <= num6)
            {
                text4 += this.HTML_VIEWPAGE(num7, "[" + num7.ToString() + "]");
                num7++;
            }
            if (nPageId < num6)
            {
                text4 = text4 + this.HTML_VIEWPAGE(nPageId + 1, "Next") + this.HTML_VIEWPAGE(num6, "Last");
            }
            text = text + text4 + "</font></p>";
            return text + "<!---SearchTag(" + this.szKeyword + ")--->";
        }
        private string HTML_VIEWPAGE(int nPageId, string szText)
        {
            return string.Concat(new string[]
			{
				"<a href='#view_page=",
				nPageId.ToString(),
				"'><span style='text-decoration: none'>",
				szText,
				"</span></a>&nbsp;&nbsp;"
			});
        }
        private string FormatSearchItem(int nId, string szName, string szDescription)
        {
            string text = "<font face='Verdana' size='2'><p style='margin-top: 5; margin-bottom: 5'><font color=red>" + nId.ToString() + ". </red><b>";
            string text2 = text;
            text = string.Concat(new string[]
			{
				text2,
				"<a href='#view_",
				this.szCategory.ToLower(),
				"=",
				szName,
				"'><span style='text-decoration: none'>"
			});
            string text3 = text;
            text = string.Concat(new string[]
			{
				text3,
				szName,
				"</b> ",
				this.szCategory,
				"</span></a></p></font>"
			});
            text += "<font face='Verdana' size=2><p style='margin-top: 5; margin-bottom: 5'>";
            text += szDescription;
            return text + "</font></p><hr size=1>\n";
        }
        public string DisplaySearchResultsFailure(string szText, string szInstead, string szCategory)
        {
            string text = "<p style='margin: 5'><font face=Verdana size=4 color=gray>";
            text += "<b>Seach Results</b></font><br><br>";
            text = text + "<font face=Verdana size=2>Your search <b><font color=brown>" + szText + "</font></b> did not match any documents <br><br>";
            text += "Suggestions:<li>Make sure all words are spelled correctly.<li>Try different keywords.";
            if (szInstead != "")
            {
                text = text + "<br><br><br>\u00a0Did you mean: <a href='#view_" + szCategory.ToLower() + "=";
                string text2 = text;
                text = string.Concat(new string[]
				{
					text2,
					szInstead,
					"'><span style='text-decoration: none'>",
					szInstead,
					"</span></a> "
				});
            }
            return text + "</font></p><!---SearchTag(" + szText + ")--->";
        }
        public int Search(string szKeyword, string szQuery)
        {
            string text = "";
            string[] array = szQuery.Split(new char[]
			{
				'|'
			});
            string str = array[0];
            this.szCategory = array[0].Substring(0, array[0].Length);
            text += "( ";
            int num = 0;
            string[] array2 = array[1].Split(new char[]
			{
				','
			});
            for (int i = 0; i < array2.Length; i++)
            {
                string text2 = array2[i];
                if (num++ > 0)
                {
                    text += " OR ";
                }
                string text3 = text;
                text = string.Concat(new string[]
				{
					text3,
					array[0],
					".",
					text2,
					" LIKE('%",
					szKeyword,
					"%')"
				});
            }
            text += " )";
            string str2 = "FROM " + str + " WHERE " + text;
            OleDbCommand oleDbCommand = new OleDbCommand("SELECT COUNT(*) " + str2, QueryDB.DbConnection);
            this.nTotalItem = int.Parse(oleDbCommand.ExecuteScalar().ToString());
            this.SQL_STATEMENT = "SELECT * " + str2;
            this.szKeyword = szKeyword;
            return this.nTotalItem;
        }
        public static void SearchWithGoogle(WebBrowser wb, string szQuery, bool bOpenNewWindow)
        {
            string text = "http://www.google.com/search?q=" + szQuery;
            if (bOpenNewWindow)
            {
                Process.Start(text);
                return;
            }
            CDisplayInfo cDisplayInfo = new CDisplayInfo();
            wb.DocumentText = cDisplayInfo.DisplayText("Searching by Google. Please wait...");
            Application.DoEvents();
            wb.Navigate(text);
        }
    }
}
