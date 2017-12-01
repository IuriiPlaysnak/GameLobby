using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

public class OutrunRealmHTMLParser : MonoBehaviour {

	void Start () {

		Debug.Log ("BlogHTMLParser");
		StartCoroutine("DownloadHTML", "http://localhost:8080/html/index.html");
	}
		
	private IEnumerator DownloadHTML(string link) {

		Debug.Log ("Loading...");

		WWW request = new WWW (link);
		yield return request;

		Debug.Log ("Loading complete");

		if (request.error != null) {

			Debug.LogError (request.error);
		} else {

			ParseHTML(request.text);
		}
	}

	private void ParseHTML(string text) {

//		string path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments) + @"\text.txt";
//		File.WriteAllText (path, text);

		/*
		XmlDocument doc = new XmlDocument ();
		doc.PreserveWhitespace = true;
		doc.LoadXml (text);
		ParseBlogPosts(doc.SelectNodes("/html/body/*[@class='WVR-blogpost']"));
		*/

//		XmlDocument doc = new XmlDocument ();
//		doc.PreserveWhitespace = true;
//		doc.LoadXml (text);
//
//		XDocument xdoc = XDocument.Parse (doc.SelectNodes("/html/body")[0].OuterXml);

		text = text.Replace ("<!DOCTYPE html>", "");

		XDocument xdoc = XDocument.Parse (text);
		var nodes = xdoc
			.Root
			.Descendants ()
			.Where (node => node.Attribute ("class") != null)
			;

		nodes
			.Where (node => node.Attribute ("class").Value == BLOG_POST_CLASS)
			.ToList ()
			.ForEach (blogpost => ParseBlogPost (blogpost));

		nodes
			.Where (node => node.Attribute ("class").Value == GALLERY_CLASS)
			.ToList ()
			.ForEach (gallery => ParseGallery(gallery));
	}

	private const string BLOG_POST_CLASS = "WVR-blogpost";
	private const string BLOG_HEADER_CLASS = "WVR-blogheader";
	private const string BLOG_IMAGE_CLASS = "WVR-blogimg";
	private const string BLOG_LINK_CLASS = "WVR-link";
	private const string GALLERY_CLASS = "WVR-gallery";

	private void ParseGallery(XElement gallery) {

		gallery
			.Descendants ()
			.Where (n => n.Name == "img")
			.ToList ()
			.ForEach (n => Debug.Log (n.Attribute("src").Value));
	}

	private void ParseBlogPost(XElement post) {

		var nodes = post
			.Descendants()
			.Where (node => node.Attribute ("class") != null)
			;

		Blogpost postData = new Blogpost();

		postData.header = 
			nodes
				.Where (node => node.Attribute ("class").Value == BLOG_HEADER_CLASS)
				.Select (node => node.Value)
				.FirstOrDefault()
				;

		postData.imageURL = 
			nodes
				.Where (node => node.Attribute ("class").Value == BLOG_IMAGE_CLASS)
				.Select (node => node.Attribute ("src").Value)
				.FirstOrDefault ()
				;

		postData.link =
			nodes
				.Where (node => node.Attribute ("class").Value == BLOG_LINK_CLASS)
				.Select (node => node.Attribute("href").Value)
				.FirstOrDefault()
				;

		Debug.Log (postData);
	}

	public struct Blogpost
	{
		public string header;
		public string imageURL;
		public string link;

		public override string ToString ()
		{
			return string.Format ("[Blogpost]: header = {0}, img = {1}, link = {2}", header, imageURL, link);
		}
	}
}
