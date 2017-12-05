using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PlaysnakRealms
{
	public class RealmsDataUtils
	{
		static public string CleanUpPlaylistURL(string originalHTML) {

			string str1 = "href=\"https://www.youtube.com/watch?";
			int index = originalHTML.IndexOf (str1);
			if (index > -1) {

				int index2 = originalHTML.IndexOf ("list=", index);
				originalHTML = originalHTML.Remove (index + str1.Length, index2 - index - str1.Length);
			}

			return originalHTML;
		}

		static public string CleanUpHTML(string originalHTML) {

			string startStr = "<div id=\"main-content\">";
			string endStr = "</div> <!-- #main-content -->";

			int startIndex = originalHTML.IndexOf (startStr);

			if (startIndex == -1)
				return originalHTML;
			
			int endIndex = originalHTML.IndexOf (endStr, startIndex) + endStr.Length;

			return originalHTML.Substring (startIndex, endIndex - startIndex);
		}

		static public string CleaupContent(string text) {

			string str = "<div class=\"et_pb_text_inner\">";
			string str2 = "</div>";

			int index = text.IndexOf (str);
			if (index > -1) {

				int index2 = text.IndexOf (str2, index);
				return text.Remove (index + str.Length, index2 - (index + str.Length));
			}

			return text;
		}

		static public string ReplaceASCIICodesWithUTF(string target)
		{
			Regex codeSequence = new Regex(@"&#[0-9]{1,4};");
			MatchCollection matches = codeSequence.Matches(target);
			StringBuilder resultStringBuilder = new StringBuilder(target);
			foreach (Match match in matches)
			{
				string matchedCodeExpression = match.Value;
				string matchedCode = matchedCodeExpression.Substring(2, matchedCodeExpression.Length - 3);
				Double resultCode = Double.Parse(matchedCode);
				resultStringBuilder.Replace(matchedCodeExpression, ((Char)resultCode).ToString());
			}
			return resultStringBuilder.ToString();
		}
	}
}

