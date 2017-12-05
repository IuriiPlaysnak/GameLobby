using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace PlaysnakRealms {

	public class RealmsFeedXMLDataSource : AbstractDataSource
	{

		#region AbstractDataSource implementation

		override public void Load (string url)
		{
			url += string.Format ("?{0}", DateTime.Now.ToLongDateString());

			OnContentItemLoadingStart ();
			OutrunRealmDataProvider.DownloadTextFile(url, ParseFeedXML);

			OnContentItemLoadingStart ();
			OutrunRealmDataProvider.DownloadTextFile(@"https://outrun.neonseoul.com/in-game/", ParseMainHTML);
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

			OnContentItemLoadingComplete();
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

			data.description = DataUtils.ReplaceASCIICodesWithUTF (descriptionText);

			gallery.images.Add (data);
		}

		private void ParseMainHTML(string text) {

			text = DataUtils.CleanUpHTML (text);
			text = DataUtils.CleanUpPlaylistURL (text);

			XDocument xdoc = XDocument.Parse (text);
			var nodes = xdoc
				.Root
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				;

			nodes
				.Where (node => node.Attribute ("class").Value == OutrunRealmHTMLDataSource.YOUTUBE_PLAYLIST_CLASS)
				.ToList ()
				.ForEach (node => ParsePlaylist(node));
		}

		private void ParsePlaylist(XElement playlistNode) {

			OutrunRealmDataProvider.PlaylistData playlist = new OutrunRealmDataProvider.PlaylistData ();
			playlist.url = playlistNode.Attribute ("href").Value;
			videos.playlists.Add (playlist);

			OnContentItemLoadingComplete ();
		}
	}
}