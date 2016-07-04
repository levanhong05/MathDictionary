using System;
using System.Collections;
using System.Data.OleDb;
using Utils.IDE.CFormat;
namespace MathDictionary
{
    internal class CDisplayInfo
    {
        public struct StructDeclare
        {
            public string szName;
            public string szType;
            public string szLib;
            public string Alias;
            public string szParam;
            public StructDeclare(string szName, string szType, string szLib, string Alias, string szParam)
            {
                this.szName = szName;
                this.szType = szType;
                this.szLib = szLib;
                this.Alias = Alias;
                this.szParam = szParam;
            }
        }

        public string[] arrLanguage = new string[]
		{
			
		};
        
        private string HTML_BlockForm(string szName, string szData)
        {
            string str = "<A class=Anchor name=Summary><FIELDSET style='WIDTH: auto' class=Property>";
            str = str + "<LEGEND class=PropertyName><font face=Verdana size=2><b>" + szName + "</b></font></LEGEND>";
            return str + "<DIV class=PropertyValue><p style='margin: 10px'>" + szData + "</p></DIV></FIELDSET></A>\n";
        }
        public string FormatUserDefineType(string szName)
        {
            string text = "";
            ArrayList arrayList = new ArrayList();
            string szSQL = "SELECT * FROM Formula WHERE Formula.Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            if (oleDbDataReader.Read())
            {
                string text2 = oleDbDataReader[0].ToString(); // Name
                string text3 = oleDbDataReader[1].ToString(); // Value
                string text4 = oleDbDataReader[2].ToString().Replace(".", "<br>\t"); //Parameter
                string text5 = oleDbDataReader[3].ToString(); //Group

                text += this.HTML_BlockForm("<font color=#blue>Value</font>", "<font color=#green>" + text2 + "</font>" + " = " + text3);
                                                
                string text6 = text;
                text = string.Concat(new string[]
				{
					text6,
                    "<b><font color=#800080>Parameter:</font></b>",
                    "<p style='margin-left: 5px'>" + text4 + "</p><br>",
				});
                text += "<b><font color=#800080>Group: </font></b>";
                text = text + "<p style='margin-left: 5px'>" + text5 + "</p><br>";
            }
            else
            {
                text = "'" + szName + "' not found !";
            }
            return text;
        }
        public string DisplayDeclareInfo(string szName)
        {
            string text = "";
            string szSQL = "SELECT * FROM Vocabulary WHERE Vocabulary.Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            if (oleDbDataReader.Read())
            {
                string text2 = oleDbDataReader[0].ToString(); //Name
                string text3 = oleDbDataReader[1].ToString(); //Meaning
                string text4 = oleDbDataReader[2].ToString(); //Description
                string text5 = oleDbDataReader[3].ToString(); //Example
                string text6 = oleDbDataReader[4].ToString(); //Solution
                string text7 = oleDbDataReader[5].ToString(); //Group
                string text8 = oleDbDataReader[6].ToString().Replace("#", ""); //Owner
                string text9 = oleDbDataReader[7].ToString().Split(new char[] //Last Edit
				{
					' '
				})[0];

                text = text + "<HTML><TITLE>" + szName + " - Math Dictionary</TITLE>\n";
                text += "<style>pre {padding: 1em;border: 1px dashed #2f6fab;color: black;background-color: #f9f9f9;line-height: 1.1em;}</style>\n";
                text += "<font face=Verdana size=2>\n";
                text = text + "&nbsp;<font size=3 color=red><b>" + szName + "</b></font><br><br>\n";
                text += this.HTML_BlockForm("<font color=#blue>Meaning</font>", "<font color=#violent>" + ((text3 == "") ? ("The " + szName + " meaning") : text3) + "</font>");
                
                text = text + "<br><b><font color=#800080>Description:</font></b><br>";
                text = text + "<p style='margin-left: 5px'>" + text4 + "</p>";
                text += "<b><font color=#800080>Example:</font></b><br><br>";
                text += "<p style='margin-left: 5px'><!---Example--->";
                text = text + ((text5 == "") ? "None." : (text5))+ "<!---Example---></p>";
                text += "<b><font color=#800080>Solution:</font></b><br><br>";
                text = text + "<p style='margin-left: 5px'>" + ((text6 == "") ? "None." : text6) + "</p>";
                text += "<b><font color=#800080>Group:</font></b><br><br>";
                text = text + "<p style='margin-left: 5px'>" + text7 + "</p>";

                if (text9 != "")
                {
                    string text11 = text;
                    text = string.Concat(new string[]
					{
						text11,
						"<p align='right'><font size=1 color=gray><i>Last update: ",
						text9,
						" - Owner: ",
						text8,
						"</i></font></p>"
					});
                }
                text = text + "</font></HTML><!---SearchTag(" + szName + ")--->";
            }
            else
            {
                text = this.DisplayText("Not found !");
            }
            return text;
        }
        public string DisplayUserDefineInfo(string szName)
        {
            string szSQL = "SELECT * FROM Formula WHERE Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            string text4;
            if (oleDbDataReader.Read())
            {
                string text = oleDbDataReader[4].ToString().Replace("#", ""); //Owner
                string text2 = oleDbDataReader[5].ToString().Split(new char[] //Last Edit
				{
					' '
				})[0];
                string text3 = this.FormatUserDefineType(szName);
                
                text4 = "<HTML><TITLE>" + szName + " - Math Dictionary</TITLE>\n";
                text4 += "<style>pre {padding: 1em;border: 1px dashed #2f6fab;color: black;background-color: #f9f9f9;line-height: 1.1em;}</style>\n";
                text4 += "<font face=Verdana size=2>\n";
                text4 = text4 + "&nbsp;<font size=3 color=red><b> Formula: " + szName + "</b></font><br><br>\n";
                text4 = text4 + "<!---Formula--->" + text3 + "<!---Formula--->";
                if (text2 != "")
                {
                    string text5 = text4;
                    text4 = string.Concat(new string[]
					{
						text5,
						"<p align='right'><font size=1 color=gray><i>Last update: ",
						text2,
						" - Owner: ",
						text,
						"</i></font></p>"
					});
                }
                text4 = text4 + "<!---SearchTag(" + szName + ")--->";
            }
            else
            {
                text4 = this.DisplayText("Not found !");
            }
            return text4;
        }

        public string DisplayConstInfo(string szName)
        {
            string text = "";
            string szSQL = "SELECT * FROM Constants WHERE Constants.Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            if (oleDbDataReader.Read())
            {
                string text2 = oleDbDataReader[0].ToString(); //Name
                string text3 = oleDbDataReader[1].ToString(); //Value
                string text4 = oleDbDataReader[2].ToString().Replace("#", ""); //Owner
                string text5 = oleDbDataReader[3].ToString().Split(new char[] //Last Edit
				{
					' '
				})[0];
                
                string source;
                
                source = string.Concat(new string[]
					{						
						"<font color=green>" + text2 + "</font>",
						" = ",
						text3,
						";"
					});
                
                text = text + "<HTML><TITLE>" + szName + " - Math Dictionary</TITLE>\n";
                text += "<style>pre {padding: 1em;border: 1px dashed #2f6fab;color: black;background-color: #f9f9f9;line-height: 1.1em;}</style>\n";
                text += "<font face=Verdana size=2>\n";
                text = text + "&nbsp;<font size=3 color=red><b>Constant: " + szName + "</b></font><br><br>\n";

                text += this.HTML_BlockForm("<font color=blue>Const </font>", source);
               
                if (text5 != "")
                {
                    string text6 = text;
                    text = string.Concat(new string[]
					{
						text6,
						"<p align='right'><font size=1 color=gray><i>Last update: ",
						text5,
						" - Owner: ",
						text4,
						"</i></font></p>"
					});
                }
                text = text + "</font><!---SearchTag(" + szName + ")--->";
            }
            else
            {
                text = this.DisplayText("Not found !");
            }
            return text;
        }
        
        public string DisplayText(string szText)
        {
            return "<br><center><font face=Verdana size=2>" + szText + "</font></center>";
        }
        public string StartPage()
        {
            string[] array = new string[]
			{
				"What is Math Dictionary ?",
				"How to using Math Dictionary ?",
				"Math Dictionary and Others Dictionary Comparison"
			};
            string text = "<center><title>Start page</title>";
            text += "<font face='Verdana' size=2><font color=red size=2><b><u>Start Page</u></b></font><br><br><br>\n";
            for (int i = 0; i < 3; i++)
            {
                string text2 = text;
                text = string.Concat(new string[]
				{
					text2,
					"<li><a href='#view_topic=tip_",
					(i + 1).ToString(),
					"'>\n<span style='text-decoration: none'>",
					array[i],
					"</a></span><br><br>\n"
				});
            }
            return text + "<br><font color=gray> Copyright © 2012 Lê Văn Hồng - University of Science</font>";
        }
        public string ViewTopic(string szName)
        {
            string result = "";
            string szSQL = "SELECT * FROM Generals WHERE Generals.Name='" + szName + "'";
            OleDbDataReader oleDbDataReader = QueryDB.DBExecuteReader(szSQL);
            if (oleDbDataReader.Read())
            {
                result = oleDbDataReader[1].ToString();
            }
            return result;
        }
    }
}