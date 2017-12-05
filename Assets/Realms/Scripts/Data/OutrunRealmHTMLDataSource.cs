using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using System;

namespace PlaysnakRealms {

	public class OutrunRealmHTMLDataSource : AbstractDataSource {
		
		private const string IMAGE_CLASS_PREFIX = "et_pb_gallery_image ";
		private const string BLOG_POST_CLASS_PREFIX = "et_pb_post ";
		public const string YOUTUBE_PLAYLIST_CLASS = "youtube_playlist";

		override public void Load (string url)
		{
			Debug.Log ("OutrunRealmHTMLDataSource");

			url += string.Format ("?{0}", DateTime.Now.ToLongDateString());
			OutrunRealmDataProvider.DownloadTextFile(url, ParseMainHTML);
		}

		private void ParseMainHTML(string text) {

			#if UNITY_EDITOR

			//System.IO.File.Create(@"d:/org.xml").Dispose();
			//System.IO.File.WriteAllText(@"d:/org.xml", text);

			//System.IO.File.Create(@"d:/blog.xml").Dispose();
			//System.IO.File.Create(@"d:/blog_org.xml").Dispose();

			#endif

			text = DataUtils.CleanUpHTML (text);
			text = DataUtils.CleanUpPlaylistURL (text);

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

			OnContentItemLoadingStart ();

			string blogPostURL = 
				blogPostNode
					.Descendants ()
					.Where (node => node.Attribute ("class") != null)
					.Where (node => node.Attribute ("class").Value == "entry-featured-image-url")
					.ElementAt (0)
					.Attribute ("href")
					.Value
					;
			
			OutrunRealmDataProvider.DownloadTextFile(blogPostURL, ParseBlogPostHTML);
		}

		private void ParseBlogPostHTML(string text) {

			#if UNITY_EDITOR
			//System.IO.File.WriteAllText(@"d:/blog_org.xml", text);
			#endif

			text = DataUtils.CleanUpHTML (text);
			text = DataUtils.CleaupContent (text);

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

			gallery.images.Insert (0, data);

			OnContentItemLoadingComplete();
		}

		private void ParseImage(XElement imageNode) {

			OnContentItemLoadingStart ();

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

			gallery.images.Add (data);

			OnContentItemLoadingComplete();
		}

		private void ParsePlaylist(XElement playlistNode) {

			OnContentItemLoadingStart ();

			OutrunRealmDataProvider.PlaylistData playlist = new OutrunRealmDataProvider.PlaylistData ();
			playlist.url = playlistNode.Attribute ("href").Value;
			videos.playlists.Add (playlist);

			OnContentItemLoadingComplete ();
		}
	}
}