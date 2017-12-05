using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using PlaysnakRealms;

namespace PlaysnakRealms {

	public class RealmsContentProvider : MonoBehaviour
	{
	    private enum SourceFormat :int
	    {
	        JSON,
	        HTML,
			RSS_XML
	    }

	    [SerializeField]
	    private SourceFormat _sourceFormat;

	    [SerializeField]
	    private string _jsonUrl = null;

	    [SerializeField]
	    private string _htmlURL = null;

		[SerializeField]
		private string _feedURL = null;



	    public static event System.Action OnLoadingComplete;

	    private static RealmsContentProvider _instance;

	    public static RealmsContentProvider instance {
	        get {
	            return _instance;
	        }
	    }

	    private bool _isLoadingComplete;
	    private SettingData _settingsData;
		private RealmsAbstractDataSource _dataSource;

	    void Awake()
	    {
	        if (_instance == null)
	            _instance = this;

	        string dataSourceUrl = string.Empty;

	        switch (_sourceFormat) {

	            case SourceFormat.JSON:

	                _dataSource = new RealmsJSONDataSource();
	                dataSourceUrl = _jsonUrl;
	                break;

	            case SourceFormat.HTML:

	                _dataSource = new RealmsHTMLDataSource();
	                dataSourceUrl = _htmlURL;
	                break;

				case SourceFormat.RSS_XML:
					_dataSource = new RealmsFeedXMLDataSource ();
					dataSourceUrl = _feedURL;
					break;

	            default:
	                _dataSource = new RealmsHTMLDataSource();
	                break;
	        }

	        _dataSource.OnLoadingComplete += OnDataLoaded;
	        _dataSource.Load(dataSourceUrl);
	    }

	    private void OnDataLoaded (SettingData data)
		{
			_settingsData = data;

			_isLoadingComplete = true;

			if (OnLoadingComplete != null)
				OnLoadingComplete ();
		}

		static public bool isLoadingComlete {
			get { return _instance._isLoadingComplete; }
		}

		static public NewsData newsData {
			get { return _instance._settingsData.newsData; }
		}

		static public GalleryData galleryData {
			get { return _instance._settingsData.galleryData; }
		}

		static public VideosData videosData {
			get { return _instance._settingsData.videosData; }
		}

		[System.Serializable]
		public struct SettingData {

			public NewsData newsData;
			public GalleryData galleryData;
			public VideosData videosData;
		}

		[System.Serializable]
		public struct VideoData {

			public string title;
			public string description;
			public string url;

			private string _id;

			public string id {

				get { 

					if (_id == "" || _id == null) {

						_id = url.Remove (0, url.IndexOf ("watch?v="));
						_id = _id.Replace ("watch?v=", "");
						int indexOfIdEnd = _id.IndexOf ("&");
						if (indexOfIdEnd > -1)
							_id = _id.Substring (0, indexOfIdEnd);
					}

					return _id;
				}
			}

			public override string ToString ()
			{
				return string.Format ("[VideoData: id={0}, url={1}]", id, url);
			}
		}

		[System.Serializable]
		public struct PlaylistData {
			public string url;
		}

		[System.Serializable]
		public struct VideosData {

			public List<VideoData> videos;
			public List<PlaylistData> playlists;
		}

		[System.Serializable]
		public struct GalleryData {

			public List<ImageData> images;
			public override string ToString ()
			{
				return string.Format ("[GalleryData]: count = {0}", images.Count);
			}
		}

		[System.Serializable]
		public struct ImageData {

			public string title;
			public string description;
			public string url;

			public override string ToString ()
			{
				return string.Format ("[ImageData]: title = {0}, descr = {1}, url = {2}", title, description, url);
			}
		}

		[System.Serializable]
		public struct NewsData {

			public string title;
			public string imageURL;
			public string link;

			public override string ToString ()
			{
				return string.Format ("[NewsData]: title = {0}, image = {1}, link = {2} ", title, imageURL, link);
			}
		}

		static public void DownloadTextFile(string link, System.Action<string> OnLoadingComplete) {

			_instance.StartCoroutine (_instance.DownloadTextFileViaWWW(link, OnLoadingComplete));
		}

		private IEnumerator DownloadTextFileViaWWW(string link, System.Action<string> OnLoadingComplete) {

			WWW request = new WWW (link);
			yield return request;

			if (request.error != null) {

				Debug.LogError (request.error);
			} else {

				OnLoadingComplete(request.text);
			}
		}
	}
}