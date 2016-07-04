using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace Utils.IDE.CFormat
{
	public abstract class SourceFormat
	{
		private byte _fontsize = 10;
		private byte _tabSpaces;
		private bool _lineNumbers;
		private bool _alternate;
		private bool _embedStyleSheet = true;
		private Regex codeRegex;
		public byte FontSize
		{
			get
			{
				return this._fontsize;
			}
			set
			{
				this._fontsize = value;
			}
		}
		public byte TabSpaces
		{
			get
			{
				return this._tabSpaces;
			}
			set
			{
				this._tabSpaces = value;
			}
		}
		public bool LineNumbers
		{
			get
			{
				return this._lineNumbers;
			}
			set
			{
				this._lineNumbers = value;
			}
		}
		public bool Alternate
		{
			get
			{
				return this._alternate;
			}
			set
			{
				this._alternate = value;
			}
		}
		public bool EmbedStyleSheet
		{
			get
			{
				return this._embedStyleSheet;
			}
			set
			{
				this._embedStyleSheet = value;
			}
		}
		protected Regex CodeRegex
		{
			get
			{
				return this.codeRegex;
			}
			set
			{
				this.codeRegex = value;
			}
		}
		protected SourceFormat()
		{
			this._tabSpaces = 4;
			this._lineNumbers = false;
			this._alternate = false;
			this._embedStyleSheet = false;
		}
		public string FormatCode(Stream source)
		{
			StreamReader streamReader = new StreamReader(source);
			string source2 = streamReader.ReadToEnd();
			streamReader.Close();
			return this.FormatCode(source2, this._lineNumbers, this._alternate, this._embedStyleSheet, false);
		}
		public string FormatCode(string source)
		{
			return this.FormatCode(source, this._lineNumbers, this._alternate, this._embedStyleSheet, false);
		}
		public string FormatSubCode(string source)
		{
			return this.FormatCode(source, false, false, false, true);
		}
		public static Stream GetCssStream()
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("Utils.IDE.CFormat.csharp.css");
		}
		public static string GetCssString()
		{
			StreamReader streamReader = new StreamReader(SourceFormat.GetCssStream());
			return streamReader.ReadToEnd();
		}
		protected abstract string MatchEval(Match match);
		private string FormatCode(string source, bool lineNumbers, bool alternate, bool embedStyleSheet, bool subCode)
		{
			StringBuilder stringBuilder = new StringBuilder(source);
			if (!subCode)
			{
				stringBuilder.Replace("&", "&amp;");
				stringBuilder.Replace("<", "&lt;");
				stringBuilder.Replace(">", "&gt;");
				stringBuilder.Replace("\t", string.Empty.PadRight((int)this._tabSpaces));
			}
			source = this.codeRegex.Replace(stringBuilder.ToString(), new MatchEvaluator(this.MatchEval));
			stringBuilder = new StringBuilder();
			if (embedStyleSheet)
			{
				stringBuilder.Append("<style type=\"text/css\">\n");
				stringBuilder.Append(".csharpcode, .csharpcode pre { font-size: " + this._fontsize.ToString() + "pt ; color: black; font-family: Consolas, Courier New, Courier, Monospace; background-color: #ffffff;/*white-space: pre;*/ } .csharpcode pre { margin: 0em; } .csharpcode .rem {color: #008000; } .csharpcode .kwrd { color: #0000ff; } .csharpcode .str {color: #990000; } .csharpcode .op { color: #0000c0; } .csharpcode .preproc {color: #cc6633; } .csharpcode .asp { background-color: #ffff00; } .csharpcode.html { color: #800000; } .csharpcode .attr { color: #ff0000; } .csharpcode .alt{ background-color: #f4f4f4; width: 100%; margin: 0em; } .csharpcode .lnum {color: #606060;}");
				stringBuilder.Append("</style>\n");
			}
			if (lineNumbers || alternate)
			{
				if (!subCode)
				{
					stringBuilder.Append("<div class=\"csharpcode\" style=\"margin-top: 5; margin-bottom: 5\">\n");
				}
				StringReader stringReader = new StringReader(source);
				int num = 0;
				string text = "    ";
				string text2;
				while ((text2 = stringReader.ReadLine()) != null)
				{
					num++;
					if (alternate && num % 2 == 1)
					{
						stringBuilder.Append("<pre class=\"alt\">");
					}
					else
					{
						stringBuilder.Append("<pre>");
					}
					if (lineNumbers)
					{
						int num2 = (int)Math.Log10((double)num);
						stringBuilder.Append("<span class=\"lnum\">" + text.Substring(0, 3 - num2) + num.ToString() + ":  </span>");
					}
					if (text2.Length == 0)
					{
						stringBuilder.Append("&nbsp;");
					}
					else
					{
						stringBuilder.Append(text2);
					}
					stringBuilder.Append("</pre>\n");
				}
				stringReader.Close();
				if (!subCode)
				{
					stringBuilder.Append("</div>");
				}
			}
			else
			{
				if (!subCode)
				{
					stringBuilder.Append("<pre class=\"csharpcode\" style=\"margin-top: 5; margin-bottom: 5\">\n");
				}
				stringBuilder.Append(source);
				if (!subCode)
				{
					stringBuilder.Append("</pre>");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
