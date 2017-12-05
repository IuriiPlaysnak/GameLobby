using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace PlaysnakRealms {

	public class RealmsFeedXMLDataSource : RealmsAbstractDataSource
	{

		#region AbstractDataSource implementation

		override public void Load (string url)
		{
			url += string.Format ("?{0}", DateTime.Now.ToLongDateString());

			LoadFeedXML (url);
			LoadVideosHTML (@"https://outrun.neonseoul.com/in-game/");
		}

		#endregion

		private void LoadFeedXML(string url) {

			OnContentItemLoadingStart ();
			RealmsContentProvider.DownloadTextFile(url, ParseFeedXML);
		}

		private void LoadVideosHTML(string url) {

			OnContentItemLoadingStart ();
			RealmsContentProvider.DownloadTextFile(url, ParseMainHTML);
		}

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
		
			RealmsContentProvider.ImageData data = new RealmsContentProvider.ImageData ();

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

			data.description = RealmsDataUtils.ReplaceASCIICodesWithUTF (descriptionText);

			gallery.images.Add (data);
		}

		private void ParseMainHTML(string text) {

			text = RealmsDataUtils.CleanUpHTML (text);
			text = RealmsDataUtils.CleanUpPlaylistURL (text);

			XDocument xdoc = XDocument.Parse (text);
			var nodes = xdoc
				.Root
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				;

			nodes
				.Where (node => node.Attribute ("class").Value == RealmsHTMLDataSource.YOUTUBE_PLAYLIST_CLASS)
				.ToList ()
				.ForEach (node => ParsePlaylist(node));

			OnContentItemLoadingComplete ();
		}

		private void ParsePlaylist(XElement playlistNode) {

			OnContentItemLoadingStart ();

			RealmsContentProvider.PlaylistData playlist = new RealmsContentProvider.PlaylistData ();
			playlist.url = playlistNode.Attribute ("href").Value;
			playlist.isForNewPlayer = playlistNode.Attribute("id") != null && playlistNode.Attribute("id").Value == "NewPlayer";
			videos.playlists.Add (playlist);

			OnContentItemLoadingComplete ();
		}
	}
}