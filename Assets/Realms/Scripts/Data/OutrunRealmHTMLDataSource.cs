using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using System;

public class OutrunRealmHTMLDataSource : IOutrunRealmDataSource {
	
	public event Action<OutrunRealmDataProvider.SettingData> OnLoadingComplete;

	private const string IMAGE_CLASS_PREFIX = "et_pb_gallery_image ";
	private const string BLOG_POST_CLASS_PREFIX = "et_pb_post ";
	private const string YOUTUBE_PLAYLIST_CLASS = "youtube_playlist";

	OutrunRealmDataProvider.GalleryData _gallery;
	OutrunRealmDataProvider.VideosData _videos;

	public void Load (string url)
	{
		Debug.Log ("OutrunRealmHTMLDataSource");

		url += string.Format ("?{0}", DateTime.Now.ToLongDateString());

		_videos = new OutrunRealmDataProvider.VideosData ();
		_videos.playlists = new List<OutrunRealmDataProvider.PlaylistData> ();

		_gallery = new OutrunRealmDataProvider.GalleryData ();
		_gallery.images = new List<OutrunRealmDataProvider.ImageData> ();

		OutrunRealmDataProvider.instance.StartCoroutine(DownloadHTML(url, ParseMainHTML));
	}

	private int _itemsAreLoading;

	private void ParseMainHTML(string text) {

		#if UNITY_EDITOR

		//System.IO.File.Create(@"d:/org.xml").Dispose();
		//System.IO.File.WriteAllText(@"d:/org.xml", text);

		//System.IO.File.Create(@"d:/blog.xml").Dispose();
		//System.IO.File.Create(@"d:/blog_org.xml").Dispose();

		#endif

		text = CleanUpHTML (text);
		text = CleanUpPlaylistURL (text);
		_itemsAreLoading = 0;

		#if UNITY_EDITOR

		//System.IO.File.Create(@"d:/clean.xml").Dispose();
		//System.IO.File.WriteAllText(@"d:/clean.xml", text);

		#endif

		XDocument xdoc = XDocument.Parse (text);
		var nodes = xdoc
			.Root
			.Descendants ()
			.Where (node => node.Attribute ("class") != null)
			;

		nodes
			.Where(node => node.Attribute("class").Value.Contains(BLOG_POST_CLASS_PREFIX))
			.ToList ()
			.ForEach (node => LoadBlogPostContent(node))
			;

		nodes
			.Where (node => node.Attribute ("class").Value == YOUTUBE_PLAYLIST_CLASS)
			.ToList ()
			.ForEach (node => ParsePlaylist(node));


//		nodes
//			.Where (node => node.Attribute ("class").Value.Contains(IMAGE_CLASS_PREFIX))
//			.ToList ()
//			.ForEach (node => ParseImage(node))
//			;

	}

	private void LoadBlogPostContent(XElement blogPostNode) {

		_itemsAreLoading++;

		string blogPostURL = 
			blogPostNode
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				.Where (node => node.Attribute ("class").Value == "entry-featured-image-url")
				.ElementAt (0)
				.Attribute ("href")
				.Value
				;

		OutrunRealmDataProvider.instance.StartCoroutine (DownloadHTML (blogPostURL, ParseBlogPostHTML));
	}

	private void ParseBlogPostHTML(string text) {

		#if UNITY_EDITOR
		//System.IO.File.WriteAllText(@"d:/blog_org.xml", text);
		#endif

		text = CleanUpHTML (text);
		text = CleaupContent (text);

		#if UNITY_EDITOR
		//System.IO.File.WriteAllText(@"d:/blog.xml", text);
		#endif

		XDocument xdoc = XDocument.Parse (text);
		xdoc
			.Descendants ()
			.Where (node => node.Attribute ("class") != null)
			.Where (node => node.Attribute ("class").Value.Contains (BLOG_POST_CLASS_PREFIX))
			.ToList ()
			.ForEach (node => ParseBlogPost (node));
	}

	private string CleaupContent(string text) {

		string str = "<div class=\"et_pb_text_inner\">";
		string str2 = "</div>";

		int index = text.IndexOf (str);
		if (index > -1) {

			int index2 = text.IndexOf (str2, index);
			return text.Remove (index + str.Length, index2 - (index + str.Length));
		}

		return text;
	}

	private void ParseBlogPost(XElement blogPostNode) {

//		Debug.Log ("parse blog");

		OutrunRealmDataProvider.ImageData data = new OutrunRealmDataProvider.ImageData();

		data.url = 
			blogPostNode
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				.Where (node => node.Attribute ("class").Value == "et_post_meta_wrapper")
				.Descendants ()
				.Where (subNode => subNode.Name == "img")
				.ElementAt(0)
				.Attribute("src")
				.Value;

		data.title = 
			blogPostNode
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				.Where (node => node.Attribute ("class").Value == "entry-title")
				.ElementAt (0)
				.Value;

		data.description =
			blogPostNode
				.Descendants ()
				.Where (node => node.Attribute ("class") != null)
				.Where (node => node.Attribute ("class").Value == "entry-content")
				.ElementAt (0)
				.Value
				.Replace ("<p>", string.Empty)
				.Replace ("</p>", string.Empty);

		_gallery.images.Insert (0, data);

		ContentItemIsLoaded();
	}

	private void ParseImage(XElement imageNode) {

		_itemsAreLoading++;

		OutrunRealmDataProvider.ImageData data = new OutrunRealmDataProvider.ImageData();

		foreach (var node in imageNode.Descendants()) {

			switch (node.Name.ToString()) {

			case "a":
				data.title = node.Attribute ("title").Value;
				break;

			case "img":
				data.url = node.Attribute ("src").Value;
				break;

			default:
				break;
			}
		}

		data.description = string.Empty;

		_gallery.images.Add (data);

		ContentItemIsLoaded();
	}

	private void ParsePlaylist(XElement playlistNode) {

		_itemsAreLoading++;

		OutrunRealmDataProvider.PlaylistData playlist = new OutrunRealmDataProvider.PlaylistData ();
		playlist.url = playlistNode.Attribute ("href").Value;
		_videos.playlists.Add (playlist);

		ContentItemIsLoaded ();
	}

	private void ContentItemIsLoaded() {
		
		_itemsAreLoading--;

		if (_itemsAreLoading == 0)
			ContentIsLoaded ();
	}

	private void ContentIsLoaded() {

		OutrunRealmDataProvider.SettingData result = new OutrunRealmDataProvider.SettingData ();
		result.galleryData = _gallery;
		result.videosData = _videos;

		if (OnLoadingComplete != null)
			OnLoadingComplete (result);
	}



	private string CleanUpPlaylistURL(string originalHTML) {

		string str1 = "href=\"https://www.youtube.com/watch?";
		int index = originalHTML.IndexOf (str1);
		if (index > -1) {

			int index2 = originalHTML.IndexOf ("list=", index);
			originalHTML = originalHTML.Remove (index + str1.Length, index2 - index - str1.Length);
		}

		return originalHTML;
	}

	private string CleanUpHTML(string originalHTML) {

		string startStr = "<div id=\"main-content\">";
		string endStr = "</div> <!-- #main-content -->";

		int startIndex = originalHTML.IndexOf (startStr);
		int endIndex = originalHTML.IndexOf (endStr, startIndex) + endStr.Length;

		return originalHTML.Substring (startIndex, endIndex - startIndex);
	}

	private IEnumerator DownloadHTML(string link, System.Action<string> OnLoadingComplete) {

//		Debug.Log ("Loading...");

		WWW request = new WWW (link);
		yield return request;

//		Debug.Log ("Loading complete");

		if (request.error != null) {

			Debug.LogError (request.error);
		} else {

			OnLoadingComplete(request.text);
		}
	}
}