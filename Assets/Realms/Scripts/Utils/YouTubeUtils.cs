public class YouTubeUtils {

	static public string GetVideoIdFromUrl(string url) {

		string result = url.Remove (0, url.IndexOf ("watch?v="));
		result = result.Replace ("watch?v=", "");
		int indexOfIdEnd = result.IndexOf ("&");
		if (indexOfIdEnd > -1)
			result = result.Substring (0, indexOfIdEnd);

		return result;
	}

	static public string GetPlaylistIdFromUrl(string url) {

		string result = url.Remove (0, url.IndexOf ("list=")).Replace("list=", "");
		int indexOfIdEnd = result.IndexOf ("&");
		if (indexOfIdEnd > -1)
			result = result.Substring (0, indexOfIdEnd);

		return result;
//		https://www.youtube.com/watch?v=nmCKUVPHK7A&list=RDnmCKUVPHK7A
	}
}
