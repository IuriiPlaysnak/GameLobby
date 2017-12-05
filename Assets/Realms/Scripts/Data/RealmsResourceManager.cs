using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace PlaysnakRealms {

	public class RealmsResourceManager : MonoBehaviour {

		static private RealmsResourceManager _instance;

		void Awake() {

			if (_instance == null)
				_instance = this;
		}

		static private Dictionary<string, List<System.Action<Texture2D>>> _texturesByCallback;
		static private Dictionary<string, Texture2D> _imagesByURLs;
		static public void LoadImage(string url, System.Action<Texture2D> onLoadigComplete) {

			if (_texturesByCallback == null)
				_texturesByCallback = new Dictionary<string, List<System.Action<Texture2D>>> ();

			if (_imagesByURLs == null)
				_imagesByURLs = new Dictionary<string, Texture2D> ();

			if (_imagesByURLs.ContainsKey (url)) {
				
				onLoadigComplete (_imagesByURLs [url]);
			} else {

				if (_texturesByCallback.ContainsKey (url) == false)
					_texturesByCallback.Add(url, new List<Action<Texture2D>>());
				
				_texturesByCallback[url].Add(onLoadigComplete);

				_instance.StartCoroutine (_instance.LoadImageCoroutine (url));
			}
		}

		private IEnumerator LoadImageCoroutine(string url) {

			WWW request = new WWW (url);

			while(request.isDone == false) {
				yield return request;
			}

			if (request.error != null) {
				Debug.LogError (request.error);
			}
			else {

				if(_imagesByURLs.ContainsKey(request.url) == false)
					_imagesByURLs.Add (request.url, request.texture);

				if (_texturesByCallback.ContainsKey (request.url)) {

					while (_texturesByCallback [request.url].Count > 0) {
						_texturesByCallback [request.url] [0] (request.texture);
						_texturesByCallback [request.url].RemoveAt (0); 
					}

				} else {

					Debug.LogError("No callback " + request.url);
				}
			}
		}

		static public WWW getCachedWWW(string url)
		{
			string filePath = Application.persistentDataPath;
	//		filePath += "/" + GetInt64HashCode(url);
			filePath += "/" + url.GetHashCode ();
			string loadFilepath = filePath;
			bool web = false;
			WWW www;
			bool useCached = false;
			useCached = System.IO.File.Exists(filePath);
			if (useCached)
			{
				//check how old
				System.DateTime written = File.GetLastWriteTimeUtc(filePath);
				System.DateTime now = System.DateTime.UtcNow;
				double totalHours = now.Subtract(written).TotalHours;
				if (totalHours > 300)
					useCached = false;
			}
			if (System.IO.File.Exists(filePath))
			{
				string pathforwww = "file://" + loadFilepath;
				Debug.Log("TRYING FROM CACHE " + url + "  file " + pathforwww);
				www = new WWW(pathforwww);
			}
			else
			{
				web = true;
				www = new WWW(url);
			}
			_instance.StartCoroutine(doLoad(www, filePath, web));
			return www;
		}

		static IEnumerator doLoad(WWW www, string filePath, bool web)
		{
			yield return www;

			if (www.error == null)
			{
				if (web)
				{
					Debug.Log("SAVING DOWNLOAD  " + www.url + " to " + filePath);
					File.WriteAllBytes(filePath, www.bytes);
					Debug.Log("SAVING DONE  " + www.url + " to " + filePath);
				}
				else
				{
					Debug.Log("SUCCESS CACHE LOAD OF " + www.url);
				}
			}
			else
			{
				if (!web)
				{
					File.Delete(filePath);
				}
				Debug.Log("WWW ERROR " + www.error);
			}
		}

		/*
		static Int64 GetInt64HashCode(string strText)
		{
			Int64 hashCode = 0;
			if (!string.IsNullOrEmpty(strText))
			{
				//Unicode Encode Covering all characterset
				byte[] byteContents = Encoding.Unicode.GetBytes(strText);
				SHA256 hash = new SHA256CryptoServiceProvider ();

	//			System.Security.Cryptography.SHA256 
	//			System.Security.Cryptography.SHA256CryptoServiceProvider

				byte[] hashText = hash.ComputeHash(byteContents);
				//32Byte hashText separate
				//hashCodeStart = 0~7  8Byte
				//hashCodeMedium = 8~23  8Byte
				//hashCodeEnd = 24~31  8Byte
				//and Fold
				Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
				Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
				Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
				hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
			}
			return (hashCode);
		}        
		*/
	}
}