using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace PlaysnakRealms {

	public class RealmsFeedXMLDataSource : IOutrunRealmDataSource
	{
		public event Action<OutrunRealmDataProvider.SettingData> OnLoadingComplete;

		OutrunRealmDataProvider.GalleryData _gallery;

		#region IOutrunRealmDataSource implementation

		void IOutrunRealmDataSource.Load (string url)
		{
			url += string.Format ("?{0}", DateTime.Now.ToLongDateString());

			_gallery = new OutrunRealmDataProvider.GalleryData ();
			_gallery.images = new List<OutrunRealmDataProvider.ImageData> ();

			OutrunRealmDataProvider.DownloadTextFile(url, ParseFeedXML);
		}

		#endregion

		private void ParseFeedXML(string text) {

			XDocument xdoc = XDocument.Parse (text);
			xdoc
				.Root
				.Descendants ()
				.Where (node => node.Name == "item")
				.ToList ()
				.ForEach (node => ParseItem(node))
				;
			
			OutrunRealmDataProvider.SettingData result = new OutrunRealmDataProvider.SettingData ();
			result.galleryData = _gallery;

			if (OnLoadingComplete != null)
				OnLoadingComplete (result);
		}

		private void ParseItem(XElement item) {
		
			OutrunRealmDataProvider.ImageData data = new OutrunRealmDataProvider.ImageData ();

			data.title = item
				.Descendants ()
				.Where (node => node.Name == "title")
				.First()
				.Value
				;

			XElement content = item
				.Descendants ()
				.Where (node => node.Name.ToString().Contains("content"))
				.First ();

			string srtString = "src=\"";

			int srcIndexFrom = content.Value.IndexOf (srtString) + srtString.Length;
			int srcIndexTo = content.Value.IndexOf ("\" ", srcIndexFrom);
			data.url = content.Value.Substring (srcIndexFrom, srcIndexTo - srcIndexFrom);
			int descriptionIndexFrom = content.Value.IndexOf ("/>", srcIndexTo);
			string descriptionText = content
				.Value
				.Substring (descriptionIndexFrom + 2)
				.Replace("<p>", string.Empty)
				.Replace("</p>", string.Empty)
				.Replace("<em>", string.Empty)
				.Replace("</em>", string.Empty)
				;


			data.description = ReplaceASCIICodesWithUTF (descriptionText);

			_gallery.images.Add (data);
		}

		private string ReplaceASCIICodesWithUTF(string target)
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