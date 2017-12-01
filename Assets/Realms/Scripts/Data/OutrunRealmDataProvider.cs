using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class OutrunRealmDataProvider : MonoBehaviour
{
    private enum SourceFormat
    {
        JSON,
        HTML
    }

    private enum SourceType
    {
        LOCAL,
        REMOTE
    }

    [SerializeField]
    private SourceType _sourceType;

    [SerializeField]
    private SourceFormat _sourceFormat;

    [SerializeField]
    private string _localPath;

    [SerializeField]
    private string _remotePath;

    [SerializeField]
    private string _jsonUrl = null;

    [SerializeField]
    private string _htmlURL = null;



    public static event System.Action OnLoadingComplete;

    private static OutrunRealmDataProvider _instance;

    public static OutrunRealmDataProvider instance {
        get {
            return _instance;
        }
    }

    private bool _isLoadingComplete;
    private SettingData _settingsData;
    private IOutrunRealmDataSource _dataSource;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        switch (_sourceType) {

            case SourceType.REMOTE:
                _jsonUrl = _remotePath;
                break;

            case SourceType.LOCAL:
            default:
                _jsonUrl = _localPath;
                break;
        }

        string dataSourceUrl = string.Empty;

        switch (_sourceFormat) {

            case SourceFormat.JSON:

                _dataSource = new OutrunRealmJSONDataSource();
                dataSourceUrl = _jsonUrl;
                break;

            case SourceFormat.HTML:

                _dataSource = new OutrunRealmHTMLDataSource();
                dataSourceUrl = _htmlURL;
                break;

            default:
                _dataSource = new OutrunRealmHTMLDataSource();
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
}