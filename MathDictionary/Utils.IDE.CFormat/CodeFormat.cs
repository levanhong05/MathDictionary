using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace Utils.IDE.CFormat
{
	public abstract class CodeFormat : SourceFormat
	{
        protected override string MatchEval(Match match)
		{
			if (match.Groups[1].Success)
			{
				StringReader stringReader = new StringReader(match.ToString());
				StringBuilder stringBuilder = new StringBuilder();
				string value;
				while ((value = stringReader.ReadLine()) != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<span class=\"rem\">");
					stringBuilder.Append(value);
					stringBuilder.Append("</span>");
				}
				return stringBuilder.ToString();
			}
			if (match.Groups[2].Success)
			{
				return "<span class=\"str\">" + match.ToString() + "</span>";
			}
			if (match.Groups[3].Success)
			{
				return "<span class=\"preproc\">" + match.ToString() + "</span>";
			}
			if (match.Groups[4].Success)
			{
				return "<span class=\"kwrd\">" + match.ToString() + "</span>";
			}
			return "";
		}
	}
}
